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
        
		internal delegate void dispatcher_t(ref uiohook_event e);
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

		[FieldOffset(4)]
		internal ulong time;
        
        // items are aligned to 4-byte boundaries by the compiler
		// also these fields are swapped (what the header says is not what is in the library)
		[FieldOffset(16)]
		internal ushort mask;

		[FieldOffset(12)]
		internal ushort reserved;

        // so this member is at byte 20
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
