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

using NUnit.Framework;
using LibUIOHookNet;
using System.Threading;
using System.Diagnostics;

namespace LibUIOHookNetTest
{
    [TestFixture()]
    public class Test
    {
        [Test()]
        public void TestCase()
        {
			UIOHook.StartHook();
			Thread.Sleep(1000);
			UIOHook.StopHook();
			Assert.AreEqual(1, Process.GetCurrentProcess().Threads.Count, "UIOHook did not shut down all threads.");
        }
	}


}
