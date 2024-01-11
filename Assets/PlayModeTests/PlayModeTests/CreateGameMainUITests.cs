using NUnit.Framework;

/* Unmerged change from project 'PlayModeTests.Player'
Before:
using System.Collections.Generic;
using NUnit.Framework;
After:
using System.Collections;
using System.Collections.Generic;
*/
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class CreateGameMainUITests
{
    // A Test behaves as an ordinary method
    [Test]
    public void CreateGameMainUITestsSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator CreateGameMainUITestsWithEnumeratorPasses()
    {
        // ToggleGroupWorldSelection
        //ToggleGroup gameObject = Resources.Load<ToggleGroup>("ToggleGroupWorldSelection");
        //var panelLogic = gameObject.GetComponent<CreateGameControllerScript>();

        //panelLogic.StartGenerationOfInfiniteRunnerGame();

        //// maybe put in the play clip duration?
        yield return new WaitForSeconds(1);

        //Assert.IsTrue(panelLogic.Warning_Not_AllOptions_Selected.activeSelf);
    }
}
