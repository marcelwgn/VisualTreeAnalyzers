using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VisualTreeAnalyzers.Core
{
    internal static class RuleScanner
    {
        private static List<Type> ruleTypes;

        public static List<IRule> GetRules(Type attributeType)
        {
            var rules = new List<IRule>();

            foreach(Type rule in LoadIRuleTypes())
            {
                if(rule.IsDefined(attributeType,false))
                {
                    try
                    {
                        var ruleImpl = (IRule)Activator.CreateInstance(rule);
                        rules.Add(ruleImpl);
                    }
                    catch (MissingMemberException) { }
                    catch (TargetInvocationException) { }
                }
            }
            return rules;
        }

        private static List<Type> LoadIRuleTypes()
        {
            if(ruleTypes == null)
            {
                var ruleInterface = typeof(IRule);
                ruleTypes = Assembly.GetExecutingAssembly().GetTypes().
                    Where(t => { return ruleInterface.IsAssignableFrom(t); }).ToList();
            }

            return ruleTypes;
        }

    }
}
