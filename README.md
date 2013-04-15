Yatzee-MBT
==========

Model based testing of the Yatzee Game using Spec Explorer.
Spec Explorer is a powerful tool allowing the tester to define a "model progam" with "actions" and "rules".
"Action" methods represent "mockup" behaviors of the real implementation under test (IUT).
"Rules" are defined on action methods and mostly indicates the conditions (or program state) under which a call to the action is a valid "move".
Spec Explorer engine compute the entire model program behaviour by exploring all possible scenario of action's valid combinations.
In practice, any search tree will be TOO wide. (test scenario for any unrestricted types such as double or string will be impractical).
The config.coord is a "coordination" file that allows to define model slicing constructs. That is, typically constraint on input ranges and specific scenario paths.
Spec Explorer output the program's behaviour model after exploration.
Then automatic code generation  of tests is offered from the behaviour model.
