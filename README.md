# OcbCargoMailFix Mod for Cities Skylines II

Attempt to fix Mail issues when a cargo hub is present in your city.

It seems that mail collected by post offices isn't sent to the
sorting facilities, as its trucks don't get dispatched properly.
Also, no outside mail for our city, as in local mail for us, is
delivered from outside via trucks, and nobody seems to
pick up the mail at the local cargo stations.

This mod disables storage and trading of LocalMail with all cargo
stations. This seems to fix the most important issue that local
consumers don't get any mail, as there is no mail to deliver.
All consumers send UnsortedMail and want to receive LocalMail.

In order to help with this further, we convert 10% of post vans from
post offices to post trucks, which seems to ease delivery of unsorted
mail to the post sorting facility.

Additionally 20% of post trucks from sorting facilities get converted
to post vans. This will allow the sorting facility to also gather and
deliver some of the local mail directly from/to end-customers.

## How to install

Install like any other BepInEx mod into the
plugins folder (extract into `BepInEx/plugins`).

## Official downloads

Please don't distribute release files to other mirror sites.
Official downloads can always be found at nexus mods or on GitHub.
Please always link back to the original sources. Thanks!

https://www.nexusmods.com/citiesskylines2/mods/102
https://github.com/OCB-CS2/OcbCargoMailFix/releases

Paypal Donations always welcome at donations@ocbnet.ch ;)