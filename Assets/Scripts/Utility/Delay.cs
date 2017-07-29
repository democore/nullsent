using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace Utility
{
    /// <summary>
    /// A simple class that allows code to be ran after a specified delay.
    /// </summary>
    public class Delay : PermaSingleton<Delay>
    {

        /// <summary>
        /// Runs the callback after a delay, this includes arguments that you can supply along with the callback.
        /// </summary>
        /// <param name="delay">The amount of time in seconds that the method will wait until running the callback.</param>
        /// <param name="callback">The callback that is ran after the specified delay.</param>
        /// <param name="varArg">The arguments that are to be used in the callback when it is called.</param>
        /// <returns>The coroutine that was started.</returns>
        public Coroutine RunCode(float delay, Action<object[]> callback, params object[] varArg)
        {
            string stacktrace = "GlobalVars.IsDebugging = false";
            if (GlobalVars.IsDebugging)
            {
                stacktrace = System.Environment.StackTrace;
            }
            return StartCoroutine(DelayRoutine(stacktrace, delay, callback, varArg));
        }

        /// <summary>
        /// Runs the callback after a delay.
        /// </summary>
        /// <param name="delay">The amount of time in seconds that the method will wait until running the callback.</param>
        /// <param name="callback">The callback that is ran after the specified delay.</param>
        /// <returns>The coroutine that was started.</returns>
        public Coroutine RunCode(float delay, Action callback)
        {
            string stacktrace = "GlobalVars.IsDebugging = false";
            if (GlobalVars.IsDebugging)
            {
                stacktrace = System.Environment.StackTrace;
            }
            return StartCoroutine(DelayRoutine(stacktrace, delay, callback));
        }

        IEnumerator DelayRoutine(string stacktrace, float delay, Action<object[]> callback, params object[] varArg)
        {
            float delayFinish = Time.time + delay;

            do
            {
                yield return null;
            } while (Time.time < delayFinish);

            try
            {
                callback(varArg);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message + "\r\n" + ex.StackTrace);
                Debug.LogError("The above Error is in delayed Code. Origin: \r\n" + stacktrace);
            }
        }

        IEnumerator DelayRoutine(string stacktrace, float delay, Action callback)
        {
            float delayFinish = Time.time + delay;

            do
            {
                yield return null;
            } while (Time.time < delayFinish);

            try
            {
                callback();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message + "\r\n" + ex.StackTrace);
                Debug.LogError("The above Error is in delayed Code. Origin: \r\n" + stacktrace);
            }
        }

        /// <summary>
        /// Stops the given Coroutine without showing an error if it's already stopped (unless seeing the exception is wanted behaviour).
        /// </summary>
        /// <param name="toStop">The Coroutine that should be stopped.</param>
        /// <param name="supressException">Should an exception be thrown if something goes wrong? (This is coded to prevent the exception shown, if the coroutine is already stopped.)</param>
        public void StopDelayedCoroutine(Coroutine toStop, bool supressException = true)
        {
            try
            {
                if (toStop != null)
                    StopCoroutine(toStop);
            }
            catch (System.Exception ex)
            {
                //The coroutine was stopped already, which is usually fine.
                if (!supressException)
                {
                    throw (ex);
                }
            }
        }
    }
}