export type MfaChallengeResult = {
  success: boolean;
  error?: string;
};

export interface MfaChallengeModalProps {
  actionTitle?: string;
  isOpen: boolean;
  onClose: () => void;
  onSuccess: (code: string) => void;
  skipApiCall?: boolean;
  onChallenge?: (code: string) => Promise<MfaChallengeResult>;
}
