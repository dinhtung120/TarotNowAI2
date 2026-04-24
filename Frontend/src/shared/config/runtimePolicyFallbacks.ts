export const RUNTIME_POLICY_FALLBACKS = {
  auth: {
    minimumAge: 18,
  },
  adminDispute: {
    defaultSplitPercentToReader: 50,
  },
  chat: {
    paymentOffer: {
      defaultAmount: 10,
      maxNoteLength: 100,
    },
    history: {
      pageSize: 50,
    },
    participants: {
      defaultPageSize: 50,
      maxPageSize: 200,
    },
  },
  realtime: {
    reconnectScheduleMs: [0, 2000, 5000, 10000, 30000],
    negotiationTimeoutMs: 8000,
    presenceNegotiationCooldownMs: 45000,
    chatUnauthorizedCooldownMs: 60000,
    serverTimeoutMs: 120000,
    chat: {
      typingClearMs: 2500,
      invalidateDebounceMs: 1000,
      initialLoadGuardMs: 2000,
      appStartGuardMs: 3000,
    },
  },
  http: {
    clientTimeoutMs: 8000,
    serverTimeoutMs: 8000,
    minTimeoutMs: 1000,
  },
  runtimePoliciesClient: {
    timeoutMs: 8000,
    staleMs: 15000,
  },
  ui: {
    readers: {
      directoryPageSize: 12,
      featuredLimit: 4,
      directoryStaleMs: 30000,
    },
    search: {
      debounceMs: 300,
    },
    prefetch: {
      chatInboxStaleMs: 30000,
    },
  },
  media: {
    upload: {
      maxImageBytes: 10 * 1024 * 1024,
      maxVoiceBytes: 5 * 1024 * 1024,
      maxVoiceDurationMs: 600000,
      imageCompressionTargetBytes: 80 * 1024,
      imageCompressionSteps: [
        { initialQuality: 0.68, maxSizeMb: 0.15, maxWidthOrHeight: 1440 },
        { initialQuality: 0.58, maxSizeMb: 0.1, maxWidthOrHeight: 1200 },
        { initialQuality: 0.48, maxSizeMb: 0.06, maxWidthOrHeight: 960 },
        { initialQuality: 0.38, maxSizeMb: 0.03, maxWidthOrHeight: 640 },
      ],
      retryAttempts: 3,
      retryDelayMs: 350,
    },
  },
} as const;
