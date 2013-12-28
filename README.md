Contrib.Persona
===============

Enable Mozilla Persona sign in for Orchard authentication (https://login.persona.org/)

Installation
============

Never test new modules in a production environment.

To install this module using the Orchard gallery, visit https://gallery.orchardproject.net/List/Modules/Orchard.Module.Contrib.Persona/1.1.0

After you enable the Contrib.Persona module, you have to ensure that one of your site Administrators has a valid e-mail.
This module will not start servicing requests unless you go to Admin->Settings->Persona and Sign in with a valid Administrator user (there is a sign in button at Persona settings).
This validation is mandatory since this module will overtake the default Orchard authentication mechanics to replace them with the very simple Persona sign in workflow.

After you verify the administrator using Persona settings, you can start testing your new Persona sign in functions. The standard logon action will create a new view (PersonaLogon instead of LogOn) informing the visitors that your site is using Mozilla Persona Sign In with some information links to Mozilla site.
Theme builders can override this view by copying the PersonaLogon.cshtml file from the module/views folder to their Theme views folder and changing it according their needs.

This module comes also with a widget you can put on your site (e.g. Navigation Zone, after your menu) to have a Persona Sign In button at the right side of your menu. If the user logins, the Widget changes to a logout button).
Theme builders can override the widget by copying the Parts.Persona.SignIn.cshtml view to their theme views folder.
Check persona.css to change positioning and colors according to your theme needs.

Persona settings
================

At this initial version you can only change the Remember Authenticated Users setting to have your site keep cookies of the authenticated users right after their sign in.

You should also set Admin->Settinggs->Users setting 'Users can create new accounts on this site' to true if you want to automatically create new users when they sign in with Persona

Administrator Validation
========================

The module keeps checking the validated administrator on every action and if any of the following happens it falls back to standard Orchard Sign In:<br>
Validated Administrator no longer exists<br>
Validated Administrator email changes<br>
Module Settings no longer exist<br>
Validated Administrator is no longer an Administrator<br>

Next version
============

Javascript functions as a jQuery plugin to be able to pass settings like Sign In and Sign Out button classes<br>
New setting to be able to change the default Persona e-mail validator. By default it uses https://verifier.login.persona.org/verify<br>
More Persona Sign In button themes out of the box<br>

Any questions or queries, please feel free to tweet me @fotisgpap, fork and improve!
