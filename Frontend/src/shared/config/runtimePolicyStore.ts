import type { RuntimePoliciesDto } from '@/shared/application/actions/runtime-policies';
import { RUNTIME_POLICY_FALLBACKS } from '@/shared/config/runtimePolicyFallbacks';

type RuntimePolicyStoreState = {
  chat: RuntimePoliciesDto['chat'];
  realtime: RuntimePoliciesDto['realtime'];
  http: RuntimePoliciesDto['http'];
  runtimePoliciesClient: RuntimePoliciesDto['runtimePoliciesClient'];
  ui: RuntimePoliciesDto['ui'];
  media: RuntimePoliciesDto['media'];
};

let currentState: RuntimePolicyStoreState = {
  chat: {
    defaultSlaHours: 12,
    allowedSlaHours: [6, 12, 24],
    maxActiveConversationsPerUser: 5,
    paymentOffer: {
      defaultAmount: RUNTIME_POLICY_FALLBACKS.chat.paymentOffer.defaultAmount,
      maxNoteLength: RUNTIME_POLICY_FALLBACKS.chat.paymentOffer.maxNoteLength,
    },
    history: {
      pageSize: RUNTIME_POLICY_FALLBACKS.chat.history.pageSize,
    },
    participants: {
      defaultPageSize: RUNTIME_POLICY_FALLBACKS.chat.participants.defaultPageSize,
      maxPageSize: RUNTIME_POLICY_FALLBACKS.chat.participants.maxPageSize,
    },
  },
  realtime: {
    reconnectScheduleMs: [...RUNTIME_POLICY_FALLBACKS.realtime.reconnectScheduleMs],
    negotiationTimeoutMs: RUNTIME_POLICY_FALLBACKS.realtime.negotiationTimeoutMs,
    presenceNegotiationCooldownMs: RUNTIME_POLICY_FALLBACKS.realtime.presenceNegotiationCooldownMs,
    chatUnauthorizedCooldownMs: RUNTIME_POLICY_FALLBACKS.realtime.chatUnauthorizedCooldownMs,
    serverTimeoutMs: RUNTIME_POLICY_FALLBACKS.realtime.serverTimeoutMs,
    chat: {
      typingClearMs: RUNTIME_POLICY_FALLBACKS.realtime.chat.typingClearMs,
      invalidateDebounceMs: RUNTIME_POLICY_FALLBACKS.realtime.chat.invalidateDebounceMs,
      initialLoadGuardMs: RUNTIME_POLICY_FALLBACKS.realtime.chat.initialLoadGuardMs,
      appStartGuardMs: RUNTIME_POLICY_FALLBACKS.realtime.chat.appStartGuardMs,
    },
  },
  http: {
    clientTimeoutMs: RUNTIME_POLICY_FALLBACKS.http.clientTimeoutMs,
    serverTimeoutMs: RUNTIME_POLICY_FALLBACKS.http.serverTimeoutMs,
    minTimeoutMs: RUNTIME_POLICY_FALLBACKS.http.minTimeoutMs,
  },
  runtimePoliciesClient: {
    timeoutMs: RUNTIME_POLICY_FALLBACKS.runtimePoliciesClient.timeoutMs,
    staleMs: RUNTIME_POLICY_FALLBACKS.runtimePoliciesClient.staleMs,
  },
  ui: {
    readers: {
      directoryPageSize: RUNTIME_POLICY_FALLBACKS.ui.readers.directoryPageSize,
      featuredLimit: RUNTIME_POLICY_FALLBACKS.ui.readers.featuredLimit,
      directoryStaleMs: RUNTIME_POLICY_FALLBACKS.ui.readers.directoryStaleMs,
    },
    search: {
      debounceMs: RUNTIME_POLICY_FALLBACKS.ui.search.debounceMs,
    },
    prefetch: {
      chatInboxStaleMs: RUNTIME_POLICY_FALLBACKS.ui.prefetch.chatInboxStaleMs,
    },
  },
  media: {
    upload: {
      maxImageBytes: RUNTIME_POLICY_FALLBACKS.media.upload.maxImageBytes,
      maxVoiceBytes: RUNTIME_POLICY_FALLBACKS.media.upload.maxVoiceBytes,
      maxVoiceDurationMs: RUNTIME_POLICY_FALLBACKS.media.upload.maxVoiceDurationMs,
      imageCompressionTargetBytes: RUNTIME_POLICY_FALLBACKS.media.upload.imageCompressionTargetBytes,
      imageCompressionSteps: RUNTIME_POLICY_FALLBACKS.media.upload.imageCompressionSteps.map((step) => ({ ...step })),
      retryAttempts: RUNTIME_POLICY_FALLBACKS.media.upload.retryAttempts,
      retryDelayMs: RUNTIME_POLICY_FALLBACKS.media.upload.retryDelayMs,
    },
  },
};

export function updateRuntimePolicyStore(policies: RuntimePoliciesDto): void {
  currentState = {
    chat: {
      ...policies.chat,
      allowedSlaHours: [...policies.chat.allowedSlaHours],
      paymentOffer: { ...policies.chat.paymentOffer },
      history: { ...policies.chat.history },
      participants: { ...policies.chat.participants },
    },
    realtime: {
      ...policies.realtime,
      reconnectScheduleMs: [...policies.realtime.reconnectScheduleMs],
      chat: { ...policies.realtime.chat },
    },
    http: { ...policies.http },
    runtimePoliciesClient: { ...policies.runtimePoliciesClient },
    ui: {
      readers: { ...policies.ui.readers },
      search: { ...policies.ui.search },
      prefetch: { ...policies.ui.prefetch },
    },
    media: {
      upload: {
        ...policies.media.upload,
        imageCompressionSteps: policies.media.upload.imageCompressionSteps.map((step) => ({ ...step })),
      },
    },
  };
}

export function getRuntimePolicyStoreSnapshot(): RuntimePolicyStoreState {
  return currentState;
}
