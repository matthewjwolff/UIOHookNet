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
using System.Threading;
namespace LibUIOHookNet
{
	public static class UIOHook
	{
		public static event EventHandler<KeyEventArgs> OnKeyPress;
		public static event EventHandler<KeyEventArgs> OnKeyRelease;

		public static event EventHandler<KeyTypeEventArgs> OnKeyType;

		public static event EventHandler<MouseEventArgs> OnMouseClick;
		public static event EventHandler<MouseEventArgs> OnMousePress;
		public static event EventHandler<MouseEventArgs> OnMouseRelease;
		public static event EventHandler<MouseEventArgs> OnMouseMove;
		public static event EventHandler<MouseEventArgs> OnMouseDrag;
		public static event EventHandler<MouseWheelEventArgs> OnMouseWheel;

		// mutex but other threads can release
		private static readonly SemaphoreSlim initializing = new SemaphoreSlim(1, 1);
		public static void StartHook()
		{
			// take the semaphore. it will be released when the EVENT_TYPE.EVENT_HOOK_ENABLED event is received
			initializing.Wait();
			Thread thread = new Thread(() =>
			{
				NativeUIOHook.hook_set_dispatch_proc(Dispatch);
				NativeUIOHook.hook_run();
				// thread is now blocked until hook_stop() is called
			})
			{
				Name = "UIOHook Dispatch Thread"
			};
			thread.Start();
			initializing.Wait();
			// finished initializing
		}

		public static void StopHook()
		{
			NativeUIOHook.hook_stop();
			// wait for shutdown event
			initializing.Wait();
			// release semaphore. the hook is cleared
			initializing.Release();
		}

		private static void Dispatch(ref uiohook_event e)
		{
			if (e.event_type == EVENT_TYPE.EVENT_HOOK_ENABLED || e.event_type == EVENT_TYPE.EVENT_HOOK_DISABLED)
			{
				initializing.Release();
			}
			else if (e.event_type == EVENT_TYPE.EVENT_KEY_PRESSED && OnKeyPress != null)
			{
				KeyEventArgs args = new KeyEventArgs(e.event_type, e.time, e.mask, e.data_keyboard.keycode);
				Thread thread = new Thread(() =>
				{
					OnKeyPress(null, args);
				});
				thread.Start();
			}
			else if (e.event_type == EVENT_TYPE.EVENT_KEY_RELEASED && OnKeyRelease!= null)
			{
				KeyEventArgs args = new KeyEventArgs(e.event_type, e.time, e.mask, e.data_keyboard.keycode);
				Thread thread = new Thread(() =>
				{
					OnKeyRelease(null, args);
				});
				thread.Start();
			}
			else if (e.event_type == EVENT_TYPE.EVENT_KEY_TYPED && OnKeyType != null)
			{
				// UNDOCUMENTED BEHAVIOR!!! e.data_keyboard.keycode will not be set, but e.data_keyboard.keychar will be (opposite for other keyboard events)
				KeyTypeEventArgs args = new KeyTypeEventArgs(e.event_type, e.time, e.mask, e.data_keyboard.keychar);
				Thread thread = new Thread(() =>
				{
					OnKeyType(null, args);
				});
				thread.Start();
			}
			else if (e.event_type == EVENT_TYPE.EVENT_MOUSE_CLICKED && OnMouseClick != null)
			{
				MouseEventArgs args = new MouseEventArgs(e.event_type, e.time, e.mask, e.reserved, e.data_mouse.button, e.data_mouse.clicks, e.data_mouse.x, e.data_mouse.y);
				Thread thread = new Thread(() =>
				{
					OnMouseClick(null, args);
				});
				thread.Start();
			}
			else if (e.event_type == EVENT_TYPE.EVENT_MOUSE_DRAGGED && OnMouseDrag != null)
			{
				MouseEventArgs args = new MouseEventArgs(e.event_type, e.time, e.mask, e.reserved, e.data_mouse.button, e.data_mouse.clicks, e.data_mouse.x, e.data_mouse.y);
				Thread thread = new Thread(() =>
				{
					OnMouseDrag(null, args);
				});
				thread.Start();
			}
			else if (e.event_type == EVENT_TYPE.EVENT_MOUSE_MOVED && OnMouseMove != null)
			{
				MouseEventArgs args = new MouseEventArgs(e.event_type, e.time, e.mask, e.reserved, e.data_mouse.button, e.data_mouse.clicks, e.data_mouse.x, e.data_mouse.y);
				Thread thread = new Thread(() =>
				{
					OnMouseMove(null, args);
				});
				thread.Start();
			}
			else if (e.event_type == EVENT_TYPE.EVENT_MOUSE_PRESSED && OnMousePress != null)
			{
				MouseEventArgs args = new MouseEventArgs(e.event_type, e.time, e.mask, e.reserved, e.data_mouse.button, e.data_mouse.clicks, e.data_mouse.x, e.data_mouse.y);
				Thread thread = new Thread(() =>
				{
					OnMousePress(null, args);
				});
				thread.Start();
			}
			else if (e.event_type == EVENT_TYPE.EVENT_MOUSE_RELEASED && OnMouseRelease != null)
			{
				MouseEventArgs args = new MouseEventArgs(e.event_type, e.time, e.mask, e.reserved, e.data_mouse.button, e.data_mouse.clicks, e.data_mouse.x, e.data_mouse.y);
				Thread thread = new Thread(() =>
				{
					OnMouseRelease(null, args);
				});
				thread.Start();
			}
			else if (e.event_type == EVENT_TYPE.EVENT_MOUSE_WHEEL && OnMouseWheel != null)
			{
				MouseWheelEventArgs args = new MouseWheelEventArgs(e.event_type, e.time, e.mask, e.reserved, e.data_wheel.clicks, e.data_wheel.x, e.data_wheel.y, e.data_wheel.type, e.data_wheel.amount, e.data_wheel.rotation);
				Thread thread = new Thread(() =>
				{
					OnMouseWheel(null, args);
				});
				thread.Start();
			}
		}
	}

	public class InputEvent
	{
		public EVENT_TYPE event_type;
		public ulong time;
		public ushort mask;

		internal InputEvent(EVENT_TYPE event_type, ulong time, ushort mask)
		{
			this.event_type = event_type;
			this.time = time;
			this.mask = mask;
		}
	}

	public class KeyEventArgs : InputEvent
	{
		public readonly ushort keycode; // TODO: this is a code defined in libuiohook.h

		internal KeyEventArgs(EVENT_TYPE event_type, ulong time, ushort mask, ushort keycode)
			: base(event_type, time, mask)
		{
			this.keycode = keycode;
		}
	}
	public class KeyTypeEventArgs : InputEvent {
		public char keychar;

		public KeyTypeEventArgs(EVENT_TYPE event_type, ulong time, ushort mask, char keychar) 
			: base(event_type, time, mask)
		{
			this.keychar = keychar;
		}
	}

	public class MouseEventArgs : InputEvent
	{
		public ushort button;
		public ushort clicks;
		public short x;
		public short y;

		internal MouseEventArgs(EVENT_TYPE event_type, ulong time, ushort mask, ushort reserved, ushort button, ushort clicks, short x, short y)
			: base(event_type, time, mask)
		{
			this.button = button;
			this.clicks = clicks;
			this.x = x;
			this.y = y;
		}
	}
	public class MouseWheelEventArgs : MouseEventArgs
	{
		public byte type;
		public ushort amount;
		public short rotation;

		internal MouseWheelEventArgs(EVENT_TYPE event_type, ulong time, ushort mask, ushort reserved, ushort clicks, short x, short y, byte type, ushort amount, short rotation)
			: base(event_type, time, mask, reserved, 0, clicks, x, y)
		{
			this.type = type;
			this.amount = amount;
			this.rotation = rotation;
		}
	}
}
