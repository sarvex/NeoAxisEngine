// Copyright (C) NeoAxis Group Ltd. 8 Copthall, Roseau Valley, 00152 Commonwealth of Dominica.
using System;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using System.Linq;
using Internal.SharpBgfx;

namespace NeoAxis
{
	/// <summary>
	/// Represents a shader generation tool for materials and effects.
	/// </summary>
	/*public */
	class ShaderGenerator
	{
		//!!!!������ �������� ��������, ����� �� ���� ������������

		string parametersNamePrefix;

		int variableNameCounter = 1;
		int parameterNameCounter = 1;

		//������������ �� ������ Process
		int textureRegisterCounter;// = 10;
								   //int samplerRegisterCounter = 10;

		Stack<VariableToCreate> variablesToCreateInQueue = new Stack<VariableToCreate>();
		//code, material index
		List<(string, int)> resultCodeLines = new List<(string, int)>();

		ESet<ShaderScript> addedShaderScripts = new ESet<ShaderScript>();

		ResultData resultData;

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Represents a result data for <see cref="ShaderGenerator"/>.
		/// </summary>
		public class ResultData
		{
			public string parametersBody = string.Empty;
			public string samplersBody = string.Empty;
			public string shaderScripts = string.Empty;
			public string codeBody = string.Empty;

			/// <summary>
			/// Represents a parameter for <see cref="ResultData"/>.
			/// </summary>
			public class ParameterItem
			{
				public ShaderParameter component;
				public Type type;
				public string nameInShader;

				//optimization
				public Uniform? uniform;
				public Type uniformType;
			}
			public List<ParameterItem> parameters;

			/// <summary>
			/// Represents a texture item for <see cref="ResultData"/>.
			/// </summary>
			public class TextureItem
			{
				public ShaderTextureSample component;
				//!!!!��� � ����������
				public string nameInShader;
				//!!!!
				public int textureRegister;
				//public int samplerRegister;
			}
			public List<TextureItem> textures;
			//!!!!new
			public List<TextureItem> texturesMask;

			//!!!!new
			/// <summary>
			/// Represents an auto constant parameter for <see cref="ResultData"/>.
			/// </summary>
			public class AutoConstantParameterItem
			{
				public Type type;
				public string nameInShader;
			}
			public Dictionary<string, AutoConstantParameterItem> autoConstantParameters;


			public void PrintToLog( string ownerDisplayName )
			{
				Log.Info( "-------------------------------------------------" );
				Log.Info( "HLSL for {0}", ownerDisplayName );
				if( !string.IsNullOrEmpty( parametersBody ) )
				{
					Log.Info( "" );
					Log.Info( "PARAMETERS BODY:" );
					Log.Info( parametersBody );
					Log.Info( "END" );
				}
				if( !string.IsNullOrEmpty( samplersBody ) )
				{
					Log.Info( "" );
					Log.Info( "SAMPLERS BODY:" );
					Log.Info( samplersBody );
					Log.Info( "END" );
				}
				if( !string.IsNullOrEmpty( shaderScripts ) )
				{
					Log.Info( "" );
					Log.Info( "SHADER SCRIPTS BODY:" );
					Log.Info( shaderScripts );
					Log.Info( "END" );
				}
				Log.Info( "" );
				Log.Info( "CODE BODY:" );
				Log.Info( codeBody );
				Log.Info( "END" );
			}

			public int GetTextureCount()
			{
				int result = 0;
				if( textures != null )
					result += textures.Count;
				if( texturesMask != null )
					result += texturesMask.Count;
				return result;
			}
		}

		///////////////////////////////////////////////

		[Browsable( false )]
		public class PaintLayerOpacityPropertyWithMask : Component
		{
			public PaintLayerOpacityPropertyWithMask()
			{
			}

			public void Init()
			{
				var sample = CreateComponent<ShaderTextureSample>();
				sample.TextureType = ShaderTextureSample.TextureTypeEnum.Mask;

				Opacity = ReferenceUtility.MakeThisReference( this, sample, "R" );
			}

			public Reference<double> Opacity
			{
				get { if( _opacity.BeginGet() ) Opacity = _opacity.Get( this ); return _opacity.value; }
				set { if( _opacity.BeginSet( ref value ) ) { _opacity.EndSet(); } }
			}
			ReferenceField<double> _opacity = 1;
		}

		///////////////////////////////////////////////

		static void GetTemplate( Component component, out string template, out Type autoConstantType, out string autoConstantName )//, out Type outputType )
		{
			autoConstantType = null;
			autoConstantName = null;
			//outputType = null;

			var invokeMember = component as InvokeMember;
			if( invokeMember != null && invokeMember.MemberObject != null )
			{
				//ShaderGenerationFunctionAttribute
				{
					var attribs = invokeMember.MemberObject.GetCustomAttributes( typeof( ShaderGenerationFunctionAttribute ), true );
					if( attribs.Length != 0 )
					{
						var attrib = (ShaderGenerationFunctionAttribute)attribs[ 0 ];
						template = attrib.Template;

						//var property = invokeMember.MemberObject as Metadata.Property;
						//if( property != null )
						//	outputType = property.Type.GetNetType();
						//var method = invokeMember.MemberObject as Metadata.Method;
						//if( method != null && method.GetReturnParameters().Length != 0 )
						//	outputType = method.GetReturnParameters()[ 0 ].Type.GetNetType();

						return;
					}
				}

				//ShaderGenerationAutoContantAttribute
				{
					var attribs = invokeMember.MemberObject.GetCustomAttributes( typeof( ShaderGenerationAutoConstantAttribute ), true );
					if( attribs.Length != 0 )
					{
						var attrib = (ShaderGenerationAutoConstantAttribute)attribs[ 0 ];
						template = attrib.Name;
						autoConstantType = attrib.Type;
						autoConstantName = attrib.Name;
						//outputType = autoConstantType;
						return;
					}
				}

				//switch( invokeMember.TempMember )
				//{
				//case InvokeMember.TEMP_MemberEnum.Double1Abs: return "abs({Value})";
				//case InvokeMember.TEMP_MemberEnum.Vec3Construct: return "vec3({X}, {Y}, {Z})";
				//case InvokeMember.TEMP_MemberEnum.Vec3Multiply: return "{V1} * {V2}";
				//}
			}

			var shaderScript = component as ShaderScript;
			if( shaderScript != null && shaderScript.CompiledScript != null )
			{
				var compiledScript = shaderScript.CompiledScript;

				var s = compiledScript.MethodName + "(";

				bool first = true;
				foreach( var p in compiledScript.MethodParameters )
				{
					if( !p.ReturnValue )
					{
						if( !first )
							s += ",";

						var name = p.Name.Substring( 0, 1 ).ToUpper() + p.Name.Substring( 1 );
						s += "{__parameter_" + name + "}";

						first = false;
					}
				}

				s += ")";

				template = s;

				//!!!!outputType?

				return;
			}

			template = "ERROR";
		}

		static List<string> GetTemplatePropertyNames( string template )
		{
			var result = new List<string>();

			for( int n = 0; n < template.Length; n++ )
			{
				if( template[ n ] == '{' )
				{
					int endN = template.IndexOf( '}', n + 1 );
					var varName = template.Substring( n + 1, endN - n - 1 );

					result.Add( varName );

					n = endN + 1;
				}
			}

			return result;
		}

		class VariableToCreate
		{
			public string name;
			public Component owner;
			public Metadata.Property property;
		}

		string GetUniqueVariableName()
		{
			var name = "var" + variableNameCounter.ToString();
			variableNameCounter++;
			return name;
		}

		string GetUniqueParameterName()
		{
			var name = "param_" + parameterNameCounter.ToString();
			parameterNameCounter++;
			return name;
		}

		int GetUniqueTextureRegisterNumber()
		{
			var value = textureRegisterCounter;
			textureRegisterCounter++;
			return value;
		}

		//int GetUniqueSamplerRegisterNumber()
		//{
		//	var value = samplerRegisterCounter;
		//	samplerRegisterCounter++;
		//	return value;
		//}

		public ResultData Process( ICollection<(Component, int, Metadata.Property)> properties, string parametersNamePrefix, Material thisMaterial, Material.CompiledMaterialData[] multiMaterialSeparateMaterialsOfCombinedGroup, ref int refTextureRegisterCounter, out string error )
		//public ResultData Process( ICollection<(Component, int, Metadata.Property)> properties, string parametersNamePrefix, Material thisMaterial, Material[] multiMaterialSeparateMaterialsOfCombinedGroup, ref int refTextureRegisterCounter, out string error )
		//public ResultData Process( Component owner, ICollection<string> propertyNames, string parametersNamePrefix, out string error )
		{
			textureRegisterCounter = refTextureRegisterCounter;

			const string returnLine = "\r";

			this.parametersNamePrefix = parametersNamePrefix;
			error = "";

			resultData = new ResultData();

			{
				//!!!!merge?

				foreach( var propertyItem in properties )
				//foreach( var propertyName in propertyNames )
				{
					var owner = propertyItem.Item1;
					var materialIndex = propertyItem.Item2;
					var property = propertyItem.Item3;

					//Metadata.Property property = (Metadata.Property)owner.MetadataGetMemberBySignature( "property:" + propertyName );
					if( property != null )
					{
						var iReference = property.GetValue( owner, null ) as IReference;
						if( iReference != null && iReference.ReferenceSpecified )
						{
							//add end line
							{
								string outputVariableName = property.Name.Substring( 0, 1 ).ToLower() + property.Name.Substring( 1 );
								string resultVariableName = "var" + variableNameCounter.ToString();
								string line = string.Format( "{0} = {1};", outputVariableName, resultVariableName );
								resultCodeLines.Add( (line, materialIndex) );
							}

							//push start variable
							{
								VariableToCreate variableToCreate = new VariableToCreate();
								variableToCreate.name = GetUniqueVariableName();
								variableToCreate.owner = owner;
								variableToCreate.property = property;
								variablesToCreateInQueue.Push( variableToCreate );
							}

							//process
							while( variablesToCreateInQueue.Count != 0 )
							{
								var variableToCreate = variablesToCreateInQueue.Pop();
								GenerateLine( variableToCreate, materialIndex );
							}
						}
					}
				}

				//parametersBody
				if( resultData.parameters != null || resultData.autoConstantParameters != null )
				{
					var s = new StringBuilder();

					if( resultData.parameters != null )
					{
						foreach( var item in resultData.parameters )
						{
							if( s.Length != 0 )
								s.Append( returnLine );
							//var typeString = GetTypeString( item.type, false );
							//!!!!bgfx
							//!!!!mat3, mat4 ���
							var line = string.Format( "uniform vec4 {0};", item.nameInShader );
							//var line = string.Format( "{0} {1};", typeString, item.nameInShader );
							s.Append( line );
						}
					}

					if( resultData.autoConstantParameters != null )
					{
						foreach( var pair in resultData.autoConstantParameters )
						{
							var item = pair.Value;

							if( s.Length != 0 )
								s.Append( returnLine );
							//var typeString = GetTypeString( item.type, false );
							//!!!!bgfx
							//!!!!mat3, mat4 ���
							var line = string.Format( "uniform vec4 {0};", item.nameInShader );
							//var line = string.Format( "{0} {1};", typeString, item.nameInShader );
							s.Append( line );
						}
					}

					resultData.parametersBody = s.ToString();
				}

				//samplersBody
				if( resultData.textures != null )
				{
					var s = new StringBuilder();
					foreach( var item in resultData.textures )
					{
						if( s.Length != 0 )
							s.Append( returnLine );

						string line;
						if( !SystemSettings.LimitedDevice )
							line = string.Format( "SAMPLER2D_TEXTUREONLY({0}, {1});", item.nameInShader, item.textureRegister );
						else
							line = string.Format( "SAMPLER2D({0}, {1});", item.nameInShader, item.textureRegister );
						s.Append( line );

						//var line1 = string.Format( "Texture2D<vec4> {0};", item.nameInShader );
						////var line1 = string.Format( "Texture2D<vec4> {0} : register( t{1} );", item.nameInShader, item.textureRegister );
						//s.Append( line1 );

						//s.Append( returnLine );
						//var line2 = string.Format( "SamplerState {0}Sampler;", item.nameInShader );
						////var line2 = string.Format( "SamplerState {0}Sampler : register( s{1} );", item.nameInShader, item.samplerRegister );
						//s.Append( line2 );
					}
					resultData.samplersBody = s.ToString();
				}

				//codeBody
				{
					var resultCodeLinesReversed = new List<(string, int)>( resultCodeLines.GetReverse() );

					//additional code to combine materials
					if( multiMaterialSeparateMaterialsOfCombinedGroup != null )
					{
						//when combined material has Opaque and Masked materials at same time, then add "opacity = 1.0;" to Opaque materials
						if( thisMaterial.BlendMode.Value == Material.BlendModeEnum.Masked )
						{
							for( int materialIndex = 0; materialIndex < multiMaterialSeparateMaterialsOfCombinedGroup.Length; materialIndex++ )
							{
								var material = multiMaterialSeparateMaterialsOfCombinedGroup[ materialIndex ].owner;
								if( material.BlendMode.Value == Material.BlendModeEnum.Opaque )
								{
									string line = "opacity = 1.0;";
									resultCodeLinesReversed.Add( (line, materialIndex) );
								}
							}
						}

						//when exists with different ShadingModel
						var containsLit = false;
						var containsSubsurface = false;
						var containsSimple = false;
						foreach( var m in multiMaterialSeparateMaterialsOfCombinedGroup )
						{
							switch( m.owner.ShadingModel.Value )
							{
							case Material.ShadingModelEnum.Lit: containsLit = true; break;
							case Material.ShadingModelEnum.Subsurface: containsSubsurface = true; break;
							case Material.ShadingModelEnum.Simple: containsSimple = true; break;
							}
						}
						if( ( ( containsLit ? 1 : 0 ) + ( containsSubsurface ? 1 : 0 ) + ( containsSimple ? 1 : 0 ) ) > 1 )
						{
							for( int materialIndex = 0; materialIndex < multiMaterialSeparateMaterialsOfCombinedGroup.Length; materialIndex++ )
							{
								var material = multiMaterialSeparateMaterialsOfCombinedGroup[ materialIndex ].owner;
								if( material.ShadingModel.Value != thisMaterial.ShadingModel.Value )
								{
									string line = $"shadingModel = {(int)material.ShadingModel.Value};";
									resultCodeLinesReversed.Add( (line, materialIndex) );
								}
							}
						}

						//when exists with different OpacityDithering
						if( thisMaterial.OpacityDithering )
						{
							for( int materialIndex = 0; materialIndex < multiMaterialSeparateMaterialsOfCombinedGroup.Length; materialIndex++ )
							{
								var material = multiMaterialSeparateMaterialsOfCombinedGroup[ materialIndex ].owner;
								if( !material.OpacityDithering )
								{
									string line = "opacityDithering = false;";
									resultCodeLinesReversed.Add( (line, materialIndex) );
								}
							}
						}

						//when exists with different RayTracingReflection
						if( thisMaterial.RayTracingReflection.Value != 0 )
						{
							for( int materialIndex = 0; materialIndex < multiMaterialSeparateMaterialsOfCombinedGroup.Length; materialIndex++ )
							{
								var material = multiMaterialSeparateMaterialsOfCombinedGroup[ materialIndex ].owner;
								if( material.RayTracingReflection.Value != 1 )
								{
									string line = $"rayTracingReflection = {material.RayTracingReflection.Value};";
									resultCodeLinesReversed.Add( (line, materialIndex) );
								}
							}
						}

						//when exists with different ReceiveDecals
						if( thisMaterial.ReceiveDecals )
						{
							for( int materialIndex = 0; materialIndex < multiMaterialSeparateMaterialsOfCombinedGroup.Length; materialIndex++ )
							{
								var material = multiMaterialSeparateMaterialsOfCombinedGroup[ materialIndex ].owner;
								if( !material.ReceiveDecals )
								{
									string line = "receiveDecals = false;";
									resultCodeLinesReversed.Add( (line, materialIndex) );
								}
							}
						}

						//when exists with different UseVertexColor
						if( thisMaterial.UseVertexColor )
						{
							for( int materialIndex = 0; materialIndex < multiMaterialSeparateMaterialsOfCombinedGroup.Length; materialIndex++ )
							{
								var material = multiMaterialSeparateMaterialsOfCombinedGroup[ materialIndex ].owner;
								if( !material.UseVertexColor )
								{
									string line = "useVertexColor = false;";
									resultCodeLinesReversed.Add( (line, materialIndex) );
								}
							}
						}


						//add if/else comparisons for material index

						var maxMaterialIndex = 0;
						foreach( var line in resultCodeLinesReversed )
						{
							if( line.Item2 > maxMaterialIndex )
								maxMaterialIndex = line.Item2;
						}
						var materialCount = maxMaterialIndex + 1;

						var ifStarted = false;

						for( int materialIndex = 0; materialIndex < materialCount; materialIndex++ )
						{
							var lines = new List<string>();
							foreach( var line in resultCodeLinesReversed )
							{
								if( line.Item2 == materialIndex )
									lines.Add( line.Item1 );
							}

							if( lines.Count != 0 )
							{
								lines.Insert( 0, string.Format( "{0}if( localGroupMaterialIndex == {1} ) ", ifStarted ? "else " : "", materialIndex ) + "{" );
								lines.Add( "}" );
								ifStarted = true;
							}

							var s = new StringBuilder();
							foreach( var line in lines )
							{
								if( s.Length != 0 )
									s.Append( returnLine );
								s.Append( line );
							}

							if( s.Length != 0 )
							{
								if( !string.IsNullOrEmpty( resultData.codeBody ) )
									resultData.codeBody += "\r\n";
								resultData.codeBody += s.ToString();
							}
						}
					}
					else
					{
						var s = new StringBuilder();
						foreach( var line in resultCodeLines.GetReverse() )
						{
							if( s.Length != 0 )
								s.Append( returnLine );
							s.Append( line.Item1 );
						}
						resultData.codeBody = s.ToString();
					}
				}
			}

			refTextureRegisterCounter = textureRegisterCounter;

			//no data
			if( string.IsNullOrEmpty( resultData.parametersBody ) && string.IsNullOrEmpty( resultData.samplersBody ) &&
				string.IsNullOrEmpty( resultData.shaderScripts ) && string.IsNullOrEmpty( resultData.codeBody ) )
			{
				return null;
			}
			return resultData;
		}

		static string TemplateToUpperParameterNames( string template )
		{
			var b = new StringBuilder();
			bool nextToUpper = false;
			foreach( var c in template )
			{
				var c2 = c;
				if( nextToUpper )
				{
					c2 = char.ToUpper( c2 );
					nextToUpper = false;
				}
				b.Append( c2 );

				if( c == '{' )
					nextToUpper = true;
			}
			return b.ToString();
		}

		void GenerateLine( VariableToCreate variableToCreate, int materialIndex )
		{
			string body;
			//Type bodyType = null;

			var propertyValue = variableToCreate.property.GetValue( variableToCreate.owner, null );
			if( propertyValue != null )
			{
				IReference iReference = propertyValue as IReference;
				if( iReference != null )
				{
					if( !string.IsNullOrEmpty( iReference.GetByReference ) )
					{
						iReference.GetMember( variableToCreate.owner, out object outObject, out Metadata.Member outMember );

						//!!!!������ ����������? ��������� �������� ���� ������?
						var component = outObject as Component;
						if( component != null )
						{
							var shaderParameter = component as ShaderParameter;
							var shaderTextureSample = component as ShaderTextureSample;
							if( shaderParameter != null )
							{
								//shader parameter

								if( shaderParameter.Property != null )
								{
									var type = shaderParameter.Property.Type.GetNetType();
									var parameterName = parametersNamePrefix + GetUniqueParameterName();

									var item = new ResultData.ParameterItem();
									item.component = shaderParameter;
									item.type = type;
									item.nameInShader = parameterName;
									if( resultData.parameters == null )
										resultData.parameters = new List<ResultData.ParameterItem>();
									resultData.parameters.Add( item );

									//!!!!new
									//add swizzles from vec4.
									//!!!!mat3, mat4
									body = parameterName + GetSwizzleFromVec4( type );
									//bodyType = type;
									//body = parameterName;

									//body = "vec3(1,0,0)";
								}
								else
								{
									//!!!!
									body = "ERROR (SHADER PARAMETER)";
								}
							}
							else if( shaderTextureSample != null )
							{
								//texture sample

								//!!!!�� ������ 2D

								//!!!!sampler: ���� ���

								bool useSourceTexture;
								if( shaderTextureSample.TextureType.Value == ShaderTextureSample.TextureTypeEnum.Mask )
									useSourceTexture = false;
								else
									useSourceTexture = !shaderTextureSample.Texture.ReferenceSpecified;

								ResultData.TextureItem item = null;
								if( !useSourceTexture )
								{
									//!!!!new
									//find item with same shaderTextureSample
									if( resultData.textures != null )
										item = resultData.textures.FirstOrDefault( i => i.component == shaderTextureSample );

									if( item == null )
									{
										//!!!!
										var textureRegister = GetUniqueTextureRegisterNumber();
										//var samplerRegister = GetUniqueSamplerRegisterNumber();
										var name = string.Format( "{0}texture_{1}", parametersNamePrefix, textureRegister );

										item = new ResultData.TextureItem();
										item.component = shaderTextureSample;
										item.nameInShader = name;
										item.textureRegister = textureRegister;
										//item.samplerRegister = samplerRegister;

										if( resultData.textures == null )
											resultData.textures = new List<ResultData.TextureItem>();
										resultData.textures.Add( item );

										//mask texture
										if( item.component.TextureType.Value == ShaderTextureSample.TextureTypeEnum.Mask )
										{
											if( resultData.texturesMask == null )
												resultData.texturesMask = new List<ResultData.TextureItem>();
											resultData.texturesMask.Add( item );
										}
									}
								}

								//bodyType = typeof( Vector4 );

								string postfix = "";
								var ar = outMember.GetCustomAttributes( typeof( ShaderGenerationPropertyPostfixAttribute ), true );
								if( ar.Length != 0 )
								{
									postfix = ( (ShaderGenerationPropertyPostfixAttribute)ar[ 0 ] ).Postfix;

									//var property = outMember as Metadata.Property;
									//if( property != null )
									//	bodyType = property.Type.GetNetType();
								}

								//!!!!Sampler
								//!!!!input.texCoord0
								//!!!!cube Location2
								string locationPropertyName = "";
								string locationStr;
								if( shaderTextureSample.Location2.ReferenceSpecified )
								{
									locationPropertyName = "Location2";
									locationStr = "{" + locationPropertyName + "}";
								}
								else
								{
									if( shaderTextureSample.TextureType.Value == ShaderTextureSample.TextureTypeEnum.Mask )
										locationStr = "unwrappedUVBeforeDisplacement";//"unwrappedUV";
									else
										locationStr = "texCoord0";
								}

								string nameInShader;
								if( !useSourceTexture )
									nameInShader = item.nameInShader;
								else
									nameInShader = "sourceTexture";

								string constructBody;
								if( shaderTextureSample.TextureType.Value == ShaderTextureSample.TextureTypeEnum.Mask )
									constructBody = string.Format( "CODE_BODY_TEXTURE2D_MASK_OPACITY({0}, {1})", nameInShader, locationStr );
								else if( shaderTextureSample.RemoveTiling )
									constructBody = string.Format( "CODE_BODY_TEXTURE2D_REMOVE_TILING({0}, {1})", nameInShader, locationStr );
								else
									constructBody = string.Format( "CODE_BODY_TEXTURE2D({0}, {1})", nameInShader, locationStr );
								//var constructBody = string.Format( "texture2D({0}, {1})", nameInShader, locationStr );
								//var constructBody = string.Format( "{0}.Sample( {1}Sampler, {2} )", nameInShader, nameInShader, locationStr );

								if( postfix != "" )
									constructBody += "." + postfix;

								var propertyNames = new List<string>();
								if( locationPropertyName != "" )
									propertyNames.Add( locationPropertyName );

								foreach( var propertyName in propertyNames )
								{
									VariableToCreate variableToCreate2 = new VariableToCreate();
									variableToCreate2.name = GetUniqueVariableName();
									variableToCreate2.owner = component;
									variableToCreate2.property = (Metadata.Property)component.MetadataGetMemberBySignature( "property:" + propertyName, new Metadata.GetMembersContext( false ) );

									variablesToCreateInQueue.Push( variableToCreate2 );

									constructBody = constructBody.Replace( "{" + propertyName + "}", variableToCreate2.name );
								}

								body = constructBody;
								//bodyType was initialized before
							}
							else
							{
								//method, property

								GetTemplate( component, out string template, out Type autoConstantType, out string autoConstantName );//, out var outputType );
								template = TemplateToUpperParameterNames( template );

								var propertyNames = GetTemplatePropertyNames( template );

								var constructBody = template;
								foreach( var propertyName in propertyNames )
								{
									VariableToCreate variableToCreate2 = new VariableToCreate();
									variableToCreate2.name = GetUniqueVariableName();
									variableToCreate2.owner = component;

									Metadata.Property p = null;
									{
										p = (Metadata.Property)component.MetadataGetMemberBySignature( "property:" + propertyName );
										//!!!!new
										if( p == null )
											p = (Metadata.Property)component.MetadataGetMemberBySignature( "property:__parameter_" + propertyName );
										if( p == null )
											p = (Metadata.Property)component.MetadataGetMemberBySignature( "property:__this_" + propertyName );
										if( p == null )
											p = (Metadata.Property)component.MetadataGetMemberBySignature( "property:__value_" + propertyName );
										if( p == null )
											p = (Metadata.Property)component.MetadataGetMemberBySignature( "property:__returnvalue_" + propertyName );
									}
									variableToCreate2.property = p;

									if( variableToCreate2.property == null )
									{
										Log.Fatal( "ShaderGenerator: GenerateLine: variableToCreate2.property == null: propertyName \'{0}\'.", propertyName );
									}
									variablesToCreateInQueue.Push( variableToCreate2 );

									constructBody = constructBody.Replace( "{" + propertyName + "}", variableToCreate2.name );
								}

								body = constructBody;
								//bodyType = outputType;

								//add auto constant item
								if( autoConstantType != null )
								{
									var item = new ResultData.AutoConstantParameterItem();
									item.type = autoConstantType;
									item.nameInShader = autoConstantName;
									if( resultData.autoConstantParameters == null )
										resultData.autoConstantParameters = new Dictionary<string, ResultData.AutoConstantParameterItem>();
									resultData.autoConstantParameters[ autoConstantName ] = item;
								}

								//add shader script to result data
								var shaderScript = component as ShaderScript;
								if( shaderScript != null && shaderScript.CompiledScript != null )
								{
									if( !addedShaderScripts.Contains( shaderScript ) )
									{
										addedShaderScripts.Add( shaderScript );

										if( resultData.shaderScripts == null )
											resultData.shaderScripts = "";
										if( !string.IsNullOrEmpty( resultData.shaderScripts ) )
											resultData.shaderScripts += "\r\n";// "\r\r";//"\r\n\r\n";
										resultData.shaderScripts += shaderScript.CompiledScript.Body;
									}
								}
							}
						}
						else
						{
							//!!!!
							body = "ERROR";
						}
					}
					else
					{
						if( iReference.ValueAsObject != null )
						{
							body = GetValueString( iReference.ValueAsObject );
							//bodyType = iReference.ValueAsObject.GetType();
						}
						else
						{
							//!!!!
							body = "NULL";
						}
					}
				}
				else
				{
					body = GetValueString( propertyValue );
					//bodyType = propertyValue.GetType();
				}
			}
			else
			{
				//!!!!
				body = "NULL";
			}

			string varTypeName;
			{
				var unrefType = ReferenceUtility.GetUnreferencedType( variableToCreate.property.Type.GetNetType() );

				bool colorValueNoAlpha = false;
				if( unrefType == typeof( ColorValue ) || unrefType == typeof( ColorValuePowered ) )
				{
					if( variableToCreate.property.GetCustomAttributes( typeof( ColorValueNoAlphaAttribute ), true ).Length != 0 )
						colorValueNoAlpha = true;
				}

				varTypeName = GetTypeString( unrefType, colorValueNoAlpha );
			}

			//compatibility fix for glsl
			//if( varTypeName == "vec3" && body.Contains( "CODE_BODY_TEXTURE2D" ) && !body.Contains( "." ) )
			//	body += ".rgb";

			string resultLine;

			//compatibility fix for glsl. convert from vec4 to vec3
			if( varTypeName == "vec3" )
				resultLine = string.Format( "{0} {1} = ({2}).xyz;", varTypeName, variableToCreate.name, body );
			else
				resultLine = string.Format( "{0} {1} = {2};", varTypeName, variableToCreate.name, body );

			//compatibility fix for glsl. convert from vec4 to vec3
			//if( varTypeName == "vec3" )
			//{
			//	if( bodyType != null && ( bodyType == typeof( ColorValue ) || bodyType == typeof( Vector4 ) || bodyType == typeof( Vector4F ) ) )
			//		resultLine = string.Format( "{0} {1} = ({2}).xyz;", varTypeName, variableToCreate.name, body );
			//}

			//if( resultLine == null )
			//	resultLine = string.Format( "{0} {1} = {2};", varTypeName, variableToCreate.name, body );

			resultCodeLines.Add( (resultLine, materialIndex) );
		}

		static string GetTypeString( Type type, bool colorValueNoAlpha )
		{
			if( type == typeof( bool ) )
				return "bool";
			if( type == typeof( int ) )
				return "int";
			if( type == typeof( uint ) )
				return "uint";
			if( type == typeof( double ) || type == typeof( float ) )
				return "float";
			if( type == typeof( Vector2 ) || type == typeof( Vector2F ) )
				return "vec2";
			if( type == typeof( Vector3 ) || type == typeof( Vector3F ) )
				return "vec3";
			if( type == typeof( Vector4 ) || type == typeof( Vector4F ) )
				return "vec4";

			if( type == typeof( ColorValue ) || type == typeof( ColorValuePowered ) )
			{
				if( colorValueNoAlpha )
					return "vec3";
				else
					return "vec4";
			}

			if( type == typeof( Matrix2 ) || type == typeof( Matrix2F ) )
				return "mat2";
			if( type == typeof( Matrix3 ) || type == typeof( Matrix3F ) )
				return "mat3";
			if( type == typeof( Matrix4 ) || type == typeof( Matrix4F ) )
				return "mat4";

			return "ERROR";
		}

		//!!!!? colorValueNoAlpha
		static string GetSwizzleFromVec4( Type type )//, bool colorValueNoAlpha )
		{
			if( type == typeof( bool ) )
				return ".x";
			if( type == typeof( double ) || type == typeof( float ) )
				return ".x";
			if( type == typeof( Vector2 ) || type == typeof( Vector2F ) )
				return ".xy";
			if( type == typeof( Vector3 ) || type == typeof( Vector3F ) )
				return ".xyz";
			if( type == typeof( Vector4 ) || type == typeof( Vector4F ) )
				return "";

			if( type == typeof( ColorValue ) || type == typeof( ColorValuePowered ) )
			{
				return "";
				//if( colorValueNoAlpha )
				//	return "vec3";
				//else
				//	return "vec4";
			}

			return ".ERROR";
		}

		static string GetValueString( object value )
		{
			if( value.GetType() == typeof( bool ) )
			{
				var v = (bool)value;
				return v ? "true" : "false";
			}
			if( value.GetType() == typeof( Vector2 ) )
			{
				var v = (Vector2)value;
				return string.Format( "vec2({0}, {1})", v.X, v.Y );
			}
			if( value.GetType() == typeof( Vector2F ) )
			{
				var v = (Vector2F)value;
				return string.Format( "vec2({0}, {1})", v.X, v.Y );
			}
			if( value.GetType() == typeof( Vector3 ) )
			{
				var v = (Vector3)value;
				return string.Format( "vec3({0}, {1}, {2})", v.X, v.Y, v.Z );
			}
			if( value.GetType() == typeof( Vector3F ) )
			{
				var v = (Vector3F)value;
				return string.Format( "vec3({0}, {1}, {2})", v.X, v.Y, v.Z );
			}
			if( value.GetType() == typeof( Vector4 ) )
			{
				var v = (Vector4)value;
				return string.Format( "vec4({0}, {1}, {2}, {3})", v.X, v.Y, v.Z, v.W );
			}
			if( value.GetType() == typeof( Vector4F ) )
			{
				var v = (Vector4F)value;
				return string.Format( "vec4({0}, {1}, {2}, {3})", v.X, v.Y, v.Z, v.W );
			}
			if( value.GetType() == typeof( ColorValue ) )
			{
				var v = (ColorValue)value;
				return string.Format( "vec4({0}, {1}, {2}, {3})", v.Red, v.Green, v.Blue, v.Alpha );
			}
			if( value.GetType() == typeof( ColorValuePowered ) )
			{
				var v = ( (ColorValuePowered)value ).ToVector4();
				return string.Format( "vec4({0}, {1}, {2}, {3})", v.X, v.Y, v.Z, v.W );
			}

			if( value is double || value is float )
			{
				var v = value.ToString();
				if( !v.Contains( "." ) )
					v += ".0";
				return v;
			}

			return value.ToString();
		}
	}
}
