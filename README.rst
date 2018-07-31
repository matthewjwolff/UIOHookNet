=========
UIOHookNet
=========

C# bindings for `libUIOHook <https://github.com/kwhat/libuiohook>`_.

This library listens for system-wide keyboard and mouse events, using C# event syntax.

Uses libUIOHook for your platform (Linux, macOS, and Windows supported)

Deviations from libUIOHook
-----------------------

The thread that calls ``StartHook`` only blocks until the hook started event has been received (internally). When that call completes, your application will begin receiving events it has subscribed to. 

When ``StopHook`` is called, it will return when your application will no longer receive events (not necessarily when all events have finished processing). 

All events are delivered on their own threads, newly created for handling that event.

License
------

Licensed under LGPL v3. See COPYING.LESSER. libUIOHook is also distributed under this license.
