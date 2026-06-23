Reproduces the issue:
Active manipulation of an element stops any other element like a button interacting, tapping the button also stops the manipulation.

Raised as Uno Platform issue #22460
https://github.com/unoplatform/uno/issues/22460

Description
Android only
Once manipulation of a visual element (the touch pad in the referenced example) has started, tapping a button on the same page will not generate any commands. When the button is tapped it halts the manipulation of the touch pad element.
After the manipulation has ended the button will generate commands normally.

Only observed on Android, works on Windows other platforms not tested.

Expected behaviour
Should work the same as Windows, Manipulation of one element should not impact other controls on the same page.

9/6/2026 - Confirmed still an issue on Uno 6.5.31
23/6/2026 - Updated to Uno 6.5.36
