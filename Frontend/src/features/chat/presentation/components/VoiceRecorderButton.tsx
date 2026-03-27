'use client';

/**
 * === VoiceRecorderButton Component ===
 *
 * Component nút ghi âm giọng nói, thay thế nút "Gửi âm thanh" (file picker) cũ.
 *
 * --- Tại sao tách thành component riêng? ---
 * 1. Component này có 2 trạng thái UI hoàn toàn khác nhau (idle vs recording)
 *    → tách riêng giúp ChatRoomPage không bị phình to
 * 2. State và animation logic (waveform, timer) chỉ liên quan đến ghi âm
 * 3. Dễ kiểm tra và tinh chỉnh UX ghi âm độc lập
 *
 * --- UX Flow ---
 * 1. Trạng thái idle: hiện icon Mic 🎤
 * 2. Bấm vào → xin quyền mic → chuyển sang recording mode
 * 3. Recording mode:
 *    - Ẩn input text + nút Send (parent xử lý)
 *    - Hiện: [Nút Hủy (X)] + [Waveform animation] + [Timer mm:ss] + [Nút Gửi (✓)]
 * 4. Bấm Gửi → dừng ghi → gọi onRecordingComplete callback
 * 5. Bấm Hủy → dừng ghi → quay lại idle
 */

import { useCallback } from 'react';
import { Loader2, Mic, Send, X } from 'lucide-react';
import {
  useVoiceRecorder,
  type VoiceRecordingResult,
} from '@/features/chat/application/useVoiceRecorder';

interface VoiceRecorderButtonProps {
  /** Callback khi ghi âm hoàn tất thành công – parent sẽ encode + gửi tin nhắn */
  onRecordingComplete: (result: VoiceRecordingResult) => void;
  /** Trạng thái disabled (ví dụ khi đang upload media khác) */
  disabled?: boolean;
}

/**
 * Format milliseconds thành chuỗi mm:ss cho timer hiển thị.
 *
 * Tại sao dùng hàm riêng thay vì inline?
 * - Logic format này có thể thay đổi (thêm giờ, thêm phần nghìn giây)
 * - Dễ test
 * - Tránh re-create logic trong mỗi lần render
 */
function formatDuration(ms: number): string {
  const totalSeconds = Math.floor(ms / 1000);
  const minutes = Math.floor(totalSeconds / 60);
  const seconds = totalSeconds % 60;
  return `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
}

export default function VoiceRecorderButton({
  onRecordingComplete,
  disabled = false,
}: VoiceRecorderButtonProps) {
  const {
    recordingState,
    isRecording,
    isRequesting,
    elapsedMs,
    audioLevels,
    errorMessage,
    startRecording,
    stopRecording,
    cancelRecording,
    dismissError,
  } = useVoiceRecorder();

  /* ========================================================================
   * handleStop – xử lý khi user bấm nút Gửi (✓) trong lúc ghi âm
   *
   * 1. Gọi stopRecording() từ hook → nhận lại Blob + durationMs
   * 2. Nếu có kết quả → gọi callback onRecordingComplete cho parent xử lý
   * 3. Parent sẽ encode thành MediaPayloadDto rồi gửi qua SignalR
   * ======================================================================== */
  const handleStop = useCallback(async () => {
    const result = await stopRecording();
    if (result) {
      onRecordingComplete(result);
    }
  }, [onRecordingComplete, stopRecording]);

  /* ========================================================================
   * Trạng thái LỖI – hiện thông báo lỗi + nút dismiss
   *
   * Lỗi phổ biến:
   * - User từ chối quyền mic trên browser
   * - Thiết bị không có microphone
   * - Trình duyệt không hỗ trợ WebM/Opus
   * ======================================================================== */
  if (recordingState === 'error') {
    return (
      <div className="flex items-center gap-2">
        <span className="text-[11px] text-[var(--danger)] max-w-[200px] truncate" title={errorMessage ?? ''}>
          {errorMessage}
        </span>
        {/* Nút dismiss – cho phép user quay lại input bình thường */}
        <button
          type="button"
          onClick={dismissError}
          className="h-9 w-9 rounded-xl bg-[var(--danger)]/20 border border-[var(--danger)]/30 text-[var(--danger)] flex items-center justify-center shrink-0"
          title="Đóng lỗi"
        >
          <X className="w-4 h-4" />
        </button>
      </div>
    );
  }

  /* ========================================================================
   * Trạng thái ĐANG GHI – hiện thanh ghi âm đầy đủ
   *
   * Layout: [Nút Hủy] [Waveform bars] [Timer] [Nút Gửi]
   *
   * Waveform animation:
   * - Mỗi bar là một div với height tính từ audioLevels (0-1)
   * - min-height 3px để luôn nhìn thấy bar dù im lặng
   * - max-height 28px để không vượt khỏi container
   * - transition smooth 80ms cho hiệu ứng mượt mà
   * - Nếu audioLevels rỗng (chưa có data) → hiện bars mặc định
   * ======================================================================== */
  if (isRecording) {
    return (
      <div className="flex items-center gap-2 flex-1 min-w-0">
        {/* Nút Hủy (X) – hủy ghi âm, quay về input text */}
        <button
          type="button"
          onClick={cancelRecording}
          className="h-11 w-11 shrink-0 rounded-xl bg-[var(--danger)]/20 border border-[var(--danger)]/30 text-[var(--danger)] flex items-center justify-center hover:bg-[var(--danger)]/30 transition-colors"
          title="Hủy ghi âm"
        >
          <X className="w-4 h-4" />
        </button>

        {/* ---------------------------------------------------------------
         * Thanh waveform + timer
         *
         * Waveform được render bằng div bars đơn giản thay vì canvas vì:
         * 1. Dễ style với CSS (border-radius, transition, colors)
         * 2. Đủ mượt cho 40 bars × 12.5fps update
         * 3. Không cần canvas API phức tạp
         *
         * Dot đỏ nhấp nháy (animate-pulse) cho cảm giác "đang ghi"
         * --------------------------------------------------------------- */}
        <div className="flex-1 flex items-center gap-2 px-3 h-11 rounded-xl bg-white/5 border border-white/10 min-w-0">
          {/* Dot đỏ nhấp nháy – indicator "đang ghi" */}
          <div className="w-2.5 h-2.5 rounded-full bg-[var(--danger)] animate-pulse shrink-0" />

          {/* Waveform visualization */}
          <div className="flex-1 flex items-center gap-[2px] h-7 min-w-0 overflow-hidden">
            {(audioLevels.length > 0 ? audioLevels : new Array(40).fill(0.05)).map(
              (level, index) => (
                <div
                  key={index}
                  className="flex-1 rounded-full bg-[var(--purple-accent)] transition-all"
                  style={{
                    /* Tính height từ biên độ: min 3px (im lặng), max 28px (to nhất) */
                    height: `${Math.max(3, Math.round(level * 28))}px`,
                    /* Opacity tăng theo biên độ: min 0.3 (luôn thấy), max 1.0 */
                    opacity: Math.max(0.3, level),
                    transitionDuration: '80ms',
                  }}
                />
              )
            )}
          </div>

          {/* Timer hiển thị thời gian đã ghi (mm:ss) */}
          <span className="text-xs text-[var(--text-secondary)] font-mono tabular-nums shrink-0 ml-1">
            {formatDuration(elapsedMs)}
          </span>
        </div>

        {/* Nút Gửi (✓) – dừng ghi và gửi tin nhắn voice */}
        <button
          type="button"
          onClick={() => void handleStop()}
          className="h-11 w-11 shrink-0 rounded-xl bg-[var(--purple-accent)] text-white flex items-center justify-center hover:brightness-110 transition-all"
          title="Gửi tin nhắn thoại"
        >
          <Send className="w-4 h-4" />
        </button>
      </div>
    );
  }

  /* ========================================================================
   * Trạng thái IDLE / REQUESTING – hiện nút Mic 🎤
   *
   * Khi bấm → gọi startRecording() → chuyển sang recording mode
   * Khi đang requesting (xin quyền mic) → hiện loading spinner
   * ======================================================================== */
  return (
    <button
      type="button"
      onClick={() => void startRecording()}
      disabled={disabled || isRequesting}
      className="h-11 w-11 shrink-0 rounded-xl bg-white/5 border border-white/10 text-[var(--text-secondary)] hover:text-white hover:bg-white/10 disabled:opacity-50 flex items-center justify-center transition-colors"
      title="Ghi âm tin nhắn thoại"
    >
      {isRequesting ? (
        <Loader2 className="w-4 h-4 animate-spin" />
      ) : (
        <Mic className="w-4 h-4" />
      )}
    </button>
  );
}
