using PeakeyeBackendCase.Models;

namespace PeakeyeBackendCase.Helpers
{
    public static class CvssCalculator
    {
        public static double Calculate(string accessVector, string accessComplexity, string authentication,
                                                 string confidentialityImpact, string integrityImpact, string availabilityImpact)
        {
            double accessVectorScore = accessVector switch
            {
                "Local" => 0.395,
                "Adjacent" => 0.646,
                "Network" => 1.0,
                _ => throw new ArgumentException("Invalid Access Vector")
            };

            double accessComplexityScore = accessComplexity switch
            {
                "High" => 0.35,
                "Medium" => 0.61,
                "Low" => 0.71,
                _ => throw new ArgumentException("Invalid Access Complexity")
            };

            double authenticationScore = authentication switch
            {
                "None" => 0.704,
                "Single" => 0.56,
                "Multiple" => 0.45,
                _ => throw new ArgumentException("Invalid Authentication")
            };

            double confidentialityImpactScore = confidentialityImpact switch
            {
                "None" => 0.0,
                "Partial" => 0.275,
                "Complete" => 0.660,
                _ => throw new ArgumentException("Invalid Confidentiality Impact")
            };

            double integrityImpactScore = integrityImpact switch
            {
                "None" => 0.0,
                "Partial" => 0.275,
                "Complete" => 0.660,
                _ => throw new ArgumentException("Invalid Integrity Impact")
            };

            double availabilityImpactScore = availabilityImpact switch
            {
                "None" => 0.0,
                "Partial" => 0.275,
                "Complete" => 0.660,
                _ => throw new ArgumentException("Invalid Availability Impact")
            };

            double impact = 10.41 * (1 - (1 - confidentialityImpactScore) * (1 - integrityImpactScore) * (1 - availabilityImpactScore));
            double exploitability = 20 * accessVectorScore * accessComplexityScore * authenticationScore;
            double fImpact = impact == 0 ? 0 : 1.176;

            return Math.Round((0.6 * impact + 0.4 * exploitability - 1.5) * fImpact, 1);
        }

        public static string DetermineSeverity(double cvssScore)
        {
            return cvssScore switch
            {
                <= 3.9 => "Low",
                <= 6.9 => "Medium",
                <= 8.9 => "High",
                _ => "Critical"
            };
        }
    }
}
