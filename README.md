# print-at-home

Printing with Epson TM-88V.

## Used printers

Some vendors set the interface on their printer to USB explicit.  
To make sure we use the network card, we need to switch it to UIB (Universal Interface Board).  
Make sure you have a shit-ton of paper and enter the settings like so:

- Turn printer off
- Hold feed button
- Turn printer on (keep holding feed)
- Receive diagnostics
- Release feed button and hold it again (Do NOT press)
- Receive menu
- Follow instructions to select options
- Go to:
-   - 3: Customize Value Settings
-   - 10: Interface Selection
- Set interface to UIB and restart the printer

_Took me lie 12 meters of paper to figure out wtf I was doing..._

## A note on printer IPs

https://support2.epson.net/manuals/english/page/aculaserc1900/adm_g/PING_2.HTM

The printes default IP was `192.168.192.168`. This meant in order to reach the network gateway, we must manually attach that subnet.

```
sudo ip addr add 192.168.192.100/24 dev enp4s0
```

Then go to the printers page (IP in browser address bar), and set the TCP to auto.  
Restart the printer and it should no longer be reahable on the default submask.

Now just remove the temp-port:

```
sudo ip addr del 192.168.192.100/24 dev enp4s0
```
