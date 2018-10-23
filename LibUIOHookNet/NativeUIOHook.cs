/* UIOHookNet: C# bindings for libUIOHook.
 * Copyright 2018 Matthew Wolff. 
 * https://github.com/matthewjwolff/UIOHookNet
 *
 * UIOHookNet is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * UIOHookNet is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Runtime.InteropServices;
namespace LibUIOHookNet
{
	class NativeUIOHook
	{
		[DllImport("uiohook")]
		internal static extern int hook_run();

		[DllImport("uiohook")]
		internal static extern int hook_stop();

		[DllImport("uiohook")]
		internal static extern void hook_set_dispatch_proc(dispatcher_t dispatch_proc);

		[DllImport("uiohook")]
		internal static extern void hook_set_logger_proc(logger_t logger_proc);
        
		internal delegate void dispatcher_t(ref uiohook_event e);

		internal delegate void logger_t(uint level, string format, IntPtr args);
    }

	#pragma warning disable 649
	struct keyboard_event_data {
		internal ushort keycode;
		internal ushort rawcode;
		internal char keychar;
	}

	struct mouse_event_data {
		internal ushort button;
		internal ushort clicks;
		internal short x;
		internal short y;
	}

	struct mouse_wheel_event_data {
		internal ushort clicks;
		internal short x;
		internal short y;
		internal byte type;
		internal ushort amount;
		internal short rotation;
	}


	[StructLayout(LayoutKind.Explicit)]
	struct uiohook_event {
		[FieldOffset(0)]
		internal EVENT_TYPE event_type;

        // This member is 8 bytes wide, so it will have been aligned to an
        // 8 byte boundary by the compiler. The previous member was only 4 
        // bytes, so 4 bytes of padding was added beteen it and this member.
		[FieldOffset(8)]
		internal ulong time;
        
		[FieldOffset(16)]
		internal ushort mask;

		[FieldOffset(18)]
		internal ushort reserved;

        // these three members are unioned, so they share the same offset.
		[FieldOffset(20)]
		internal keyboard_event_data data_keyboard; 
		[FieldOffset(20)]
		internal mouse_event_data data_mouse;
		[FieldOffset(20)]
		internal mouse_wheel_event_data data_wheel;
	}
	#pragma warning restore 649

	public enum EVENT_TYPE // int
    {
        EVENT_HOOK_ENABLED = 1,
        EVENT_HOOK_DISABLED,
        EVENT_KEY_TYPED,
        EVENT_KEY_PRESSED,
        EVENT_KEY_RELEASED,
        EVENT_MOUSE_CLICKED,
        EVENT_MOUSE_PRESSED,
        EVENT_MOUSE_RELEASED,
        EVENT_MOUSE_MOVED,
        EVENT_MOUSE_DRAGGED,
        EVENT_MOUSE_WHEEL
    }
}
