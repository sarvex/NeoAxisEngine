//------------------------------------------------------------------------------
// Common math
//------------------------------------------------------------------------------

/** @public-api */
#ifndef PI
#define PI                 3.14159265359
#endif

/** @public-api */
#ifndef HALF_PI
#define HALF_PI            1.570796327
#endif

#define MEDIUMP_FLT_MAX    65504.0
#define MEDIUMP_FLT_MIN    0.00006103515625

#ifdef MOBILE
	#define FLT_EPS            MEDIUMP_FLT_MIN
	#define saturateMediump(x) min(x, MEDIUMP_FLT_MAX)
#else
	#define FLT_EPS            1e-5
	#define saturateMediump(x) x
#endif

//------------------------------------------------------------------------------
// Scalar operations
//------------------------------------------------------------------------------

/**
 * Computes x^5 using only multiply operations.
 *
 * @public-api
 */
float pow5(float x) {
    float x2 = x * x;
    return x2 * x2 * x;
}

/**
 * Computes x^2 as a single multiplication.
 *
 * @public-api
 */
float sq(float x) {
    return x * x;
}

/**
 * Returns the maximum component of the specified vector.
 *
 * @public-api
 */
float max3(const vec3 v) {
    return max(v.x, max(v.y, v.z));
}

//------------------------------------------------------------------------------
// Matrix and quaternion operations
//------------------------------------------------------------------------------

/**
 * Multiplies the specified 3-component vector by the 4x4 matrix (m * v) in
 * high precision.
 *
 * @public-api
 */
//vec4 mulMat4x4Float3(const HIGHP mat4 m, const HIGHP vec3 v) {
//    return v.x * m[0] + (v.y * m[1] + (v.z * m[2] + m[3]));
//}

/**
 * Multiplies the specified 3-component vector by the 3x3 matrix (m * v) in
 * high precision.
 *
 * @public-api
 */
//vec3 mulMat3x3Float3(const HIGHP mat4 m, const HIGHP vec3 v) {
//    return v.x * m[0].xyz + (v.y * m[1].xyz + (v.z * m[2].xyz));
//}

/**
 * Extracts the normal vector of the tangent frame encoded in the specified quaternion.
 */
//void toTangentFrame(const HIGHP vec4 q, out HIGHP vec3 n) {
//    n = vec3( 0.0,  0.0,  1.0) +
//        vec3( 2.0, -2.0, -2.0) * q.x * q.zwx +
//        vec3( 2.0,  2.0, -2.0) * q.y * q.wzy;
//}

/**
 * Extracts the normal and tangent vectors of the tangent frame encoded in the
 * specified quaternion.
 */
//void toTangentFrame(const HIGHP vec4 q, out HIGHP vec3 n, out HIGHP vec3 t) {
//    toTangentFrame(q, n);
//    t = vec3( 1.0,  0.0,  0.0) +
//        vec3(-2.0,  2.0, -2.0) * q.y * q.yxw +
//        vec3(-2.0,  2.0,  2.0) * q.z * q.zwx;
//}

/*
vec3 halfPartialTransformVertexUnitQ(const vec3 v, const vec4 q) {
    // this work only for unit-quaternions
    return cross(q.xyz, cross(q.xyz, v) + q.w * v);
}

vec3 transformVertexUnitQ(const vec3 v, const vec4 q) {
    // this work only for unit-quaternions
    return v + 2.0 * halfPartialTransformVertexUnitQ(v, q);
}

vec3 transformVertexUnitQT(const vec3 v, const vec4 q, const vec3 t) {
    // this work only for unit-quaternions
    return transformVertexUnitQ(v, q) + t;
}

vec3 partialTransformVertexUnitQT(const vec3 v, const vec4 q, const vec3 t) {
    // this work only for unit-quaternions
    return 2.0f * halfPartialTransformVertexUnitQ(v, q) + t;
}
*/
