#if !NO_UI_WEB_BROWSER
//
// This file manually written from cef/include/internal/cef_string_list.h.
//
namespace Internal.Xilium.CefGlue.Interop
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    internal static unsafe partial class libcef
    {
        // Allocate a new string list.
        [DllImport(DllName, EntryPoint = "cef_string_list_alloc", CallingConvention = CEF_CALL)]
        public static extern cef_string_list* string_list_alloc();

        // Return the number of elements in the string list.
        [DllImport(DllName, EntryPoint = "cef_string_list_size", CallingConvention = CEF_CALL)]
        public static extern int string_list_size(cef_string_list* list);

        // Retrieve the value at the specified zero-based string list index. Returns
        // true (1) if the value was successfully retrieved.
        [DllImport(DllName, EntryPoint = "cef_string_list_value", CallingConvention = CEF_CALL)]
        public static extern int string_list_value(cef_string_list* list, int index, cef_string_t* value);

        // Append a new value at the end of the string list.
        [DllImport(DllName, EntryPoint = "cef_string_list_append", CallingConvention = CEF_CALL)]
        public static extern void string_list_append(cef_string_list* list, cef_string_t* value);

        // Clear the string list.
        [DllImport(DllName, EntryPoint = "cef_string_list_clear", CallingConvention = CEF_CALL)]
        public static extern void string_list_clear(cef_string_list* list);

        // Free the string list.
        [DllImport(DllName, EntryPoint = "cef_string_list_free", CallingConvention = CEF_CALL)]
        public static extern void string_list_free(cef_string_list* list);

        // Creates a copy of an existing string list.
        [DllImport(DllName, EntryPoint = "cef_string_list_copy", CallingConvention = CEF_CALL)]
        public static extern cef_string_list* string_list_copy(cef_string_list* list);
    }
}

#endif