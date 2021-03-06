﻿namespace SqsAll
{
    using NServiceBus;

    public static class SqsTransportConfigurationExtensions
    {
        internal const string RegionEnvironmentVariableName = "NServiceBus.AmazonSQS.Region";
        const string S3BucketEnvironmentVariableName = "NServiceBus.AmazonSQS.S3Bucket";
        const string NativeDeferralEnvironmentVariableName = "NServiceBus.AmazonSQS.NativeDeferral";

        public static string S3BucketName => EnvironmentHelper.GetEnvironmentVariable(S3BucketEnvironmentVariableName);

        public static void ConfigureSqsTransport(this TransportExtensions<SqsTransport> transportConfiguration, string queueNamePrefix = null)
        {
            var region = EnvironmentHelper.GetEnvironmentVariable(RegionEnvironmentVariableName) ?? "ap-southeast-2";

            transportConfiguration
                .Region(region)
                .QueueNamePrefix(queueNamePrefix);

            var s3BucketName = EnvironmentHelper.GetEnvironmentVariable(S3BucketEnvironmentVariableName);

            if (!string.IsNullOrEmpty(S3BucketName))
            {
                transportConfiguration.S3BucketForLargeMessages(S3BucketName, "test");
            }

            var nativeDeferralRaw = EnvironmentHelper.GetEnvironmentVariable(NativeDeferralEnvironmentVariableName);
            var validValue = bool.TryParse(nativeDeferralRaw, out var nativeDeferral);
            if (validValue && nativeDeferral)
            {
                transportConfiguration.NativeDeferral();
            }
        }
    }
}