'use client';

import { useCallback } from 'react';
import { useVoiceRecorder, type VoiceRecordingResult } from '@/features/chat/application/useVoiceRecorder';
import VoiceRecorderActiveInline from '@/features/chat/presentation/components/voice-recorder/VoiceRecorderActiveInline';
import VoiceRecorderErrorInline from '@/features/chat/presentation/components/voice-recorder/VoiceRecorderErrorInline';
import VoiceRecorderStartButton from '@/features/chat/presentation/components/voice-recorder/VoiceRecorderStartButton';

interface VoiceRecorderButtonProps {
  disabled?: boolean;
  onRecordingComplete: (result: VoiceRecordingResult) => void;
}

export default function VoiceRecorderButton({ onRecordingComplete, disabled = false }: VoiceRecorderButtonProps) {
  const { recordingState, isRecording, isRequesting, elapsedMs, audioLevels, errorMessage, startRecording, stopRecording, cancelRecording, dismissError } = useVoiceRecorder();

  const handleStop = useCallback(async () => {
    const result = await stopRecording();
    if (result) onRecordingComplete(result);
  }, [onRecordingComplete, stopRecording]);

  if (recordingState === 'error') {
    return <VoiceRecorderErrorInline errorMessage={errorMessage} onDismiss={dismissError} />;
  }

  if (isRecording) {
    return <VoiceRecorderActiveInline audioLevels={audioLevels} elapsedMs={elapsedMs} onCancel={cancelRecording} onSend={() => void handleStop()} />;
  }

  return <VoiceRecorderStartButton disabled={disabled} isRequesting={isRequesting} onStart={() => void startRecording()} />;
}
