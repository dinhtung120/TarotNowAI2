'use client';

import { useCallback, useEffect, useRef, useState } from 'react';
import { useVoiceRecorder, type VoiceRecordingResult } from '@/features/chat/room/components/useVoiceRecorder';
import VoiceRecorderActiveInline from '@/features/chat/room/components/VoiceRecorderActiveInline';
import VoiceRecorderErrorInline from '@/features/chat/room/components/VoiceRecorderErrorInline';
import { Plus, Image as ImageIcon, Mic, Loader2 } from 'lucide-react';
import { cn } from '@/lib/utils';

interface VoiceRecorderButtonProps {
  disabled?: boolean;
  onRecordingComplete: (result: VoiceRecordingResult) => void;
  onImageClick?: () => void;
}

export default function VoiceRecorderButton({ onRecordingComplete, onImageClick, disabled = false }: VoiceRecorderButtonProps) {
  const { recordingState, isRecording, isRequesting, elapsedMs, audioLevels, errorMessage, startRecording, stopRecording, cancelRecording, dismissError } = useVoiceRecorder();
  const [showMenu, setShowMenu] = useState(false);
  const menuRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (menuRef.current && !menuRef.current.contains(event.target as Node)) {
        setShowMenu(false);
      }
    }
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

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

  return (
    <div className="relative" ref={menuRef}>
      <button
        type="button"
        disabled={disabled || isRequesting}
        onClick={() => setShowMenu(!showMenu)}
        className={cn(
          'flex h-11 w-11 shrink-0 items-center justify-center rounded-xl border border-white/10 bg-white/5 transition-colors text-[var(--text-secondary)] hover:text-white',
          (showMenu || isRequesting) && 'bg-white/10 text-white',
          disabled && 'opacity-50 cursor-not-allowed'
        )}
        title="Đa phương tiện"
        aria-label="Đa phương tiện"
      >
        {isRequesting ? <Loader2 className="h-4 w-4 animate-spin" /> : <Plus className="h-4 w-4" />}
      </button>

      {showMenu && !disabled && (
        <div className="absolute bottom-14 left-0 z-20 w-40 overflow-hidden rounded-xl border border-white/10 bg-zinc-900/95 p-1 shadow-xl backdrop-blur-md">
          {onImageClick && (
            <button
              type="button"
              className="flex w-full items-center gap-2 rounded-lg px-3 py-2 text-sm text-zinc-300 transition-colors hover:bg-white/10 hover:text-white"
              onClick={() => {
                setShowMenu(false);
                onImageClick();
              }}
            >
              <ImageIcon className="h-4 w-4" />
              Gửi ảnh
            </button>
          )}
          <button
            type="button"
            className="flex w-full items-center gap-2 rounded-lg px-3 py-2 text-sm text-zinc-300 transition-colors hover:bg-white/10 hover:text-white"
            onClick={() => {
              setShowMenu(false);
              void startRecording();
            }}
          >
            <Mic className="h-4 w-4" />
            Ghi âm
          </button>
        </div>
      )}
    </div>
  );
}
