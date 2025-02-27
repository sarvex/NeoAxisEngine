///* API declaration export attribute */
//#define AL_API  __declspec(dllexport)
//#define ALC_API __declspec(dllexport)

/* Finds the current platform */
#if (defined( __WIN32__ ) || defined( _WIN32 )) && !defined(__ANDROID__)
#   include <sdkddkver.h>
#   if defined(WINAPI_FAMILY)
#       include <winapifamily.h>
#       if WINAPI_FAMILY == WINAPI_FAMILY_APP|| WINAPI_FAMILY == WINAPI_FAMILY_PHONE_APP
#           define PLATFORM_WINRT
#       else
#           define PLATFORM_WINDOWS
#       endif
#   else
#       define PLATFORM_WINDOWS
#   endif
#elif defined(IOS)
#define PLATFORM_IOS
#elif defined( __APPLE_CC__)
#define PLATFORM_MACOS
#elif defined(__ANDROID__)
#define PLATFORM_ANDROID
#else
#error Platform is not supported.
#endif

#if defined(PLATFORM_WINDOWS) || defined(PLATFORM_WINRT)
#define AL_API  __declspec(dllexport)
#define ALC_API __declspec(dllexport)
#else
#define AL_API  __attribute__ ((visibility("default")))
#define ALC_API __attribute__ ((visibility("default")))
#endif

/* Define a restrict macro for non-aliased pointers */
#define RESTRICT __restrict

/* Define if HRTF data is embedded in the library */
#define ALSOFT_EMBED_HRTF_DATA

/* Define if we have the posix_memalign function */
/* #undef HAVE_POSIX_MEMALIGN */

/* Define if we have the _aligned_malloc function */
#if defined(PLATFORM_WINDOWS) || defined(PLATFORM_WINRT)
#define HAVE__ALIGNED_MALLOC
#endif

/* Define if we have the proc_pidpath function */
/* #undef HAVE_PROC_PIDPATH */

/* Define if we have the getopt function */
/* #undef HAVE_GETOPT */

/* Define if we have SSE CPU extensions */
#if defined(PLATFORM_WINDOWS) || defined(PLATFORM_WINRT)
#define HAVE_SSE
#define HAVE_SSE2
#define HAVE_SSE3
#endif
//#define HAVE_SSE4_1

/* Define if we have ARM Neon CPU extensions */
/* #undef HAVE_NEON */

/* Define if we have the ALSA backend */
/* #undef HAVE_ALSA */

/* Define if we have the OSS backend */
/* #undef HAVE_OSS */

/* Define if we have the Solaris backend */
/* #undef HAVE_SOLARIS */

/* Define if we have the SndIO backend */
/* #undef HAVE_SNDIO */

/* Define if we have the QSA backend */
/* #undef HAVE_QSA */

/* Define if we have the WASAPI backend */
#ifdef PLATFORM_WINDOWS
#define HAVE_WASAPI
#endif

/* Define if we have the DSound backend */
//#define HAVE_DSOUND

/* Define if we have the Windows Multimedia backend */
//#define HAVE_WINMM

/* Define if we have the PortAudio backend */
/* #undef HAVE_PORTAUDIO */

/* Define if we have the PulseAudio backend */
/* #undef HAVE_PULSEAUDIO */

/* Define if we have the JACK backend */
/* #undef HAVE_JACK */

/* Define if we have the CoreAudio backend */
#ifdef PLATFORM_IOS
#define HAVE_COREAUDIO
#endif
/* #undef HAVE_COREAUDIO */

/* Define if we have the OpenSL backend */
#ifdef PLATFORM_ANDROID
#define HAVE_OPENSL
#endif
/* #undef HAVE_OPENSL */

/* Define if we have the Wave Writer backend */
//#define HAVE_WAVE

/* Define if we have the SDL2 backend */
#ifdef PLATFORM_WINRT
#define HAVE_SDL2
#endif

/* Define if we have the stat function */
#define HAVE_STAT

/* Define to the size of a long int type */
#define SIZEOF_LONG 4

/* Define to the size of a long long int type */
#define SIZEOF_LONG_LONG 8

/* Define if we have GCC's format attribute */
/* #undef HAVE_GCC_FORMAT */

/* Define if we have dlfcn.h */
/* #undef HAVE_DLFCN_H */

/* Define if we have pthread_np.h */
/* #undef HAVE_PTHREAD_NP_H */

#ifndef PLATFORM_IOS
/* Define if we have malloc.h */
#define HAVE_MALLOC_H
#endif

/* Define if we have dirent.h */
/* #undef HAVE_DIRENT_H */

/* Define if we have cpuid.h */
/* #undef HAVE_CPUID_H */

/* Define if we have intrin.h */
#if defined(PLATFORM_WINDOWS) || defined(PLATFORM_WINRT)
#define HAVE_INTRIN_H
#endif

/* Define if we have sys/sysconf.h */
/* #undef HAVE_SYS_SYSCONF_H */

/* Define if we have guiddef.h */
#define HAVE_GUIDDEF_H

/* Define if we have initguid.h */
/* #undef HAVE_INITGUID_H */

/* Define if we have GCC's __get_cpuid() */
/* #undef HAVE_GCC_GET_CPUID */

/* Define if we have the __cpuid() intrinsic */
#define HAVE_CPUID_INTRINSIC

/* Define if we have the _BitScanForward64() intrinsic */
/* #undef HAVE_BITSCANFORWARD64_INTRINSIC */

/* Define if we have the _BitScanForward() intrinsic */
#define HAVE_BITSCANFORWARD_INTRINSIC

/* Define if we have SSE intrinsics */
#if defined(PLATFORM_WINDOWS) || defined(PLATFORM_WINRT)
#define HAVE_SSE_INTRINSICS
#endif

/* Define if we have pthread_setschedparam() */
/* #undef HAVE_PTHREAD_SETSCHEDPARAM */

/* Define if we have pthread_setname_np() */
/* #undef HAVE_PTHREAD_SETNAME_NP */

/* Define if pthread_setname_np() only accepts one parameter */
/* #undef PTHREAD_SETNAME_NP_ONE_PARAM */

/* Define if pthread_setname_np() accepts three parameters */
/* #undef PTHREAD_SETNAME_NP_THREE_PARAMS */

/* Define if we have pthread_set_name_np() */
/* #undef HAVE_PTHREAD_SET_NAME_NP */
