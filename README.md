# PowerSleepWake

## About

This is a module for PowerShell 7 that provides cdlets to sleep the
current PC and wake another PC via Wake-On-Lan.

This project was inspired by a personal need of mine for scripting, but
is also an excercise in learning to write PowerShell cmdlets.

This module contains two cmdlets.

* `Enter-Sleep` will put the current system to sleep. This is only functional on Windows. It takes no parameters.
* `Send-WakeOnLan` will send a Wake-On-Lan packet to the specified MAC address. It takes one paramter: the MAC address to send the WOL packet to.

## Copyright

Copyright 2023 [Miff](https://miffthefox.info/)

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
