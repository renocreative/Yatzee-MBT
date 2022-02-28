Yatzee-MBT
==========

_Model-based testing of an implementation of the Yatzee Game Rules using Spec Explorer._


**Spec Explorer** is a powerful tool allowing the tester to define a model progam with _actions_ and _rules_: 
- _Action_ methods represent mockup behaviors of the real Implementation Under Test (IUT). 
- _Rules_ are defined on action methods and mostly indicates the conditions (or program state) under which a call to the action is a valid "move".

**Spec Explorer**'s engine computes the entire model program behaviour by exploring all possible scenarios of valid combinations of actions.\
In practice, any search tree will be too wide. For example, a test scenario for any unrestricted types such as double or string will be impractical. The `config.cord` is a "coordination" file that allows to define _model slicing_ constructs. These are typically constrained on input ranges and specific scenario paths. Spec Explorer outputs the program's behaviour model after exploration.
From this, code generation of tests is offered.
