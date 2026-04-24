namespace TarotNow.Application.Features.UserContext.Queries.GetRuntimePolicies;

public sealed partial class GetRuntimePoliciesQueryHandler
{
    private RuntimeRealtimePolicyDto BuildRealtimePolicy()
    {
        return new RuntimeRealtimePolicyDto
        {
            ReconnectScheduleMs = _systemConfigSettings.RealtimeReconnectScheduleMs,
            NegotiationTimeoutMs = _systemConfigSettings.RealtimeNegotiationTimeoutMs,
            PresenceNegotiationCooldownMs = _systemConfigSettings.RealtimePresenceNegotiationCooldownMs,
            ChatUnauthorizedCooldownMs = _systemConfigSettings.RealtimeChatUnauthorizedCooldownMs,
            ServerTimeoutMs = _systemConfigSettings.RealtimeServerTimeoutMs,
            Chat = new RuntimeRealtimeChatPolicyDto
            {
                TypingClearMs = _systemConfigSettings.RealtimeChatTypingClearMs,
                InvalidateDebounceMs = _systemConfigSettings.RealtimeChatInvalidateDebounceMs,
                InitialLoadGuardMs = _systemConfigSettings.RealtimeChatInitialLoadGuardMs,
                AppStartGuardMs = _systemConfigSettings.RealtimeChatAppStartGuardMs
            }
        };
    }

    private RuntimeHttpPolicyDto BuildHttpPolicy()
    {
        return new RuntimeHttpPolicyDto
        {
            ClientTimeoutMs = _systemConfigSettings.OperationalHttpClientTimeoutMs,
            ServerTimeoutMs = _systemConfigSettings.OperationalHttpServerTimeoutMs,
            MinTimeoutMs = _systemConfigSettings.OperationalHttpMinTimeoutMs
        };
    }

    private RuntimeRuntimePoliciesClientPolicyDto BuildRuntimePoliciesClientPolicy()
    {
        return new RuntimeRuntimePoliciesClientPolicyDto
        {
            TimeoutMs = _systemConfigSettings.OperationalRuntimePoliciesTimeoutMs,
            StaleMs = _systemConfigSettings.OperationalRuntimePoliciesStaleMs
        };
    }

    private RuntimeUiPolicyDto BuildUiPolicy()
    {
        return new RuntimeUiPolicyDto
        {
            Readers = new RuntimeUiReadersPolicyDto
            {
                DirectoryPageSize = _systemConfigSettings.UiReadersDirectoryPageSize,
                FeaturedLimit = _systemConfigSettings.UiReadersFeaturedLimit,
                DirectoryStaleMs = _systemConfigSettings.UiReadersDirectoryStaleMs
            },
            Search = new RuntimeUiSearchPolicyDto
            {
                DebounceMs = _systemConfigSettings.UiSearchDebounceMs
            },
            Prefetch = new RuntimeUiPrefetchPolicyDto
            {
                ChatInboxStaleMs = _systemConfigSettings.UiPrefetchChatInboxStaleMs
            }
        };
    }

    private RuntimeMediaPolicyDto BuildMediaPolicy()
    {
        return new RuntimeMediaPolicyDto
        {
            Upload = new RuntimeMediaUploadPolicyDto
            {
                MaxImageBytes = _systemConfigSettings.MediaUploadMaxImageBytes,
                MaxVoiceBytes = _systemConfigSettings.MediaUploadMaxVoiceBytes,
                MaxVoiceDurationMs = _systemConfigSettings.MediaUploadMaxVoiceDurationMs,
                ImageCompressionTargetBytes = _systemConfigSettings.MediaUploadImageCompressionTargetBytes,
                ImageCompressionSteps = _systemConfigSettings.MediaUploadImageCompressionSteps
                    .Select(step => new RuntimeMediaImageCompressionStepDto
                    {
                        InitialQuality = step.InitialQuality,
                        MaxSizeMb = step.MaxSizeMb,
                        MaxWidthOrHeight = step.MaxWidthOrHeight
                    })
                    .ToArray(),
                RetryAttempts = _systemConfigSettings.MediaUploadRetryAttempts,
                RetryDelayMs = _systemConfigSettings.MediaUploadRetryDelayMs
            }
        };
    }
}
