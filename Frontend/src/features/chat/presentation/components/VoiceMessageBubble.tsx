'use client';

/**
 * === VoiceMessageBubble Component ===
 *
 * Component hiển thị tin nhắn thoại (voice message) đã gửi/nhận trong chat.
 *
 * --- Tại sao không dùng <audio controls> mặc định? ---
 * 1. <audio controls> có giao diện khác nhau trên mỗi trình duyệt
 *    → không nhất quán và không khớp dark theme của app
 * 2. Thiết kế yêu cầu waveform + nút Play tùy chỉnh (Chat.md line 250)
 * 3. Cần hiển thị progress bar khi đang phát
 * 4. Cần hiển thị duration tĩnh khi chưa phát
 *
 * --- Cách hoạt động ---
 * 1. Render waveform tĩnh (fake bars ngẫu nhiên dựa trên deterministic seed)
 * 2. Khi bấm Play → tạo HTMLAudioElement → phát audio từ data URL
 * 3. Cập nhật progress bar real-time qua requestAnimationFrame
 * 4. Khi phát xong hoặc bấm Pause → dừng
 */

import { useCallback, useEffect, useRef, useState } from 'react';
import { Pause, Play } from 'lucide-react';

interface VoiceMessageBubbleProps {
  /** Data URL (base64) của audio – lấy từ mediaPayload.url */
  audioUrl: string;
  /** Thời lượng tính bằng milliseconds (từ mediaPayload.durationMs) */
  durationMs?: number | null;
  /** Tin nhắn của mình (true) hay đối phương (false) – ảnh hưởng màu sắc */
  isMe: boolean;
}

/**
 * Số lượng bars trong waveform tĩnh.
 * 30 bars cân bằng giữa visual density và performance.
 */
const STATIC_BAR_COUNT = 30;

/**
 * Tạo mảng waveform bars tĩnh (deterministic) từ một seed string.
 *
 * Tại sao dùng deterministic thay vì random?
 * - Random sẽ thay đổi mỗi lần re-render → waveform nhảy lung tung
 * - Seed từ audioUrl đảm bảo cùng một tin nhắn luôn có cùng waveform
 * - Mỗi lần mở lại chat, waveform vẫn trông giống hệt
 *
 * Thuật toán: hash từng ký tự của seed → pseudo-random 0-1 cho mỗi bar
 */
function generateStaticBars(seed: string): number[] {
  const bars: number[] = [];
  /* Dùng hash đơn giản: cộng charCode × position modulo prime number
   * Prime 31 là số hay dùng trong string hashing (như Java hashCode)
   * Kết quả chia 100 để normalize về [0, 1], clamp để đảm bảo giới hạn */
  let hash = 0;
  for (let i = 0; i < seed.length; i++) {
    hash = ((hash << 5) - hash + seed.charCodeAt(i)) | 0;
  }

  for (let i = 0; i < STATIC_BAR_COUNT; i++) {
    /* Pseudo-random dựa trên hash + index
     * sin() cho phân bố khá đều, * 10000 rồi lấy phần thập phân → 0-1 */
    const value = Math.abs(Math.sin(hash + i * 31) * 10000) % 1;
    /* Clamp: tối thiểu 0.15 (bar luôn hiện), tối đa 0.9 (không quá cao) */
    bars.push(Math.max(0.15, Math.min(0.9, value)));
  }

  return bars;
}

/**
 * Format milliseconds thành chuỗi mm:ss dễ đọc.
 */
function formatDuration(ms: number): string {
  const totalSeconds = Math.max(0, Math.floor(ms / 1000));
  const minutes = Math.floor(totalSeconds / 60);
  const seconds = totalSeconds % 60;
  return `${minutes}:${seconds.toString().padStart(2, '0')}`;
}

export default function VoiceMessageBubble({ audioUrl, durationMs, isMe }: VoiceMessageBubbleProps) {
  /* ========================================================================
   * STATE
   * - playing: audio đang phát hay không
   * - progress: tiến trình phát từ 0 đến 1 (cho progress bar)
   * - currentTimeMs: thời gian hiện tại đang phát (ms) – hiển thị timer
   * - resolvedDurationMs: thời lượng thực tế (từ metadata audio hoặc prop)
   * ======================================================================== */
  const [playing, setPlaying] = useState(false);
  const [progress, setProgress] = useState(0);
  const [currentTimeMs, setCurrentTimeMs] = useState(0);
  const [resolvedDurationMs, setResolvedDurationMs] = useState(durationMs ?? 0);

  /* ========================================================================
   * REFS
   * - audioRef: HTMLAudioElement instance, tạo 1 lần rồi reuse
   * - animationFrameRef: ID của requestAnimationFrame, cần để cancel khi stop
   * - barsData: waveform bars tĩnh, tạo 1 lần từ seed
   * ======================================================================== */
  const audioRef = useRef<HTMLAudioElement | null>(null);
  const animationFrameRef = useRef<number | null>(null);
  const barsData = useRef(generateStaticBars(audioUrl));

  /* ========================================================================
   * updateProgress – cập nhật progress bar trong khi audio đang phát
   *
   * Dùng requestAnimationFrame thay vì setInterval vì:
   * 1. Đồng bộ với refresh rate màn hình → mượt hơn
   * 2. Tự động pause khi tab không active → tiết kiệm CPU
   * 3. Không bị drift như setInterval
   * ======================================================================== */
  const updateProgress = useCallback(() => {
    const audio = audioRef.current;
    if (!audio || audio.paused) return;

    const duration = audio.duration;
    if (Number.isFinite(duration) && duration > 0) {
      setProgress(audio.currentTime / duration);
      setCurrentTimeMs(Math.round(audio.currentTime * 1000));
    }

    animationFrameRef.current = requestAnimationFrame(updateProgress);
  }, []);

  /* ========================================================================
   * dataUrlToBlobUrl – Chuyển đổi data URL (base64) thành Blob URL
   *
   * Tại sao cần chuyển đổi?
   * - Nhiều trình duyệt (đặc biệt Safari, Firefox) KHÔNG hỗ trợ phát audio
   *   từ data URL dài (hàng MB base64) → ném NotSupportedError
   * - Blob URL (blob:http://...) là tham chiếu trực tiếp tới dữ liệu nhị phân
   *   trong bộ nhớ, trình duyệt xử lý hiệu quả hơn rất nhiều
   * - Blob URL cũng giảm memory footprint vì không cần giữ string base64
   *
   * Luồng: data:audio/webm;base64,... → decode base64 → Uint8Array → Blob → URL
   * ======================================================================== */
  const blobUrlRef = useRef<string | null>(null);

  const ensureBlobUrl = useCallback((): string => {
    /* Nếu đã convert rồi thì tái sử dụng, không convert lại */
    if (blobUrlRef.current) return blobUrlRef.current;

    /* Nếu audioUrl đã là blob: hoặc http: thì dùng trực tiếp */
    if (!audioUrl.startsWith('data:')) {
      return audioUrl;
    }

    /* Tách phần header (data:audio/webm;base64,) và phần data (base64 string) */
    const commaIndex = audioUrl.indexOf(',');
    if (commaIndex < 0) return audioUrl;

    const header = audioUrl.substring(0, commaIndex);
    const base64Data = audioUrl.substring(commaIndex + 1);

    /* Trích xuất MIME type từ header, ví dụ: "data:audio/webm;base64" → "audio/webm" */
    const mimeMatch = header.match(/data:([^;]+)/);
    const mimeType = mimeMatch ? mimeMatch[1] : 'audio/webm';

    /* Decode base64 → binary string → Uint8Array */
    const binaryString = atob(base64Data);
    const bytes = new Uint8Array(binaryString.length);
    for (let i = 0; i < binaryString.length; i++) {
      bytes[i] = binaryString.charCodeAt(i);
    }

    /* Tạo Blob từ binary data rồi tạo Object URL */
    const blob = new Blob([bytes], { type: mimeType });
    const url = URL.createObjectURL(blob);
    blobUrlRef.current = url;
    return url;
  }, [audioUrl]);

  /* ========================================================================
   * togglePlay – chuyển đổi Play/Pause
   *
   * Lần đầu phát: convert data URL → Blob URL → tạo Audio element
   * Lần sau: pause/play element đã có
   *
   * Tại sao tạo Audio element bằng JS thay vì <audio> trong JSX?
   * - Không cần render element vào DOM (ẩn nó đi)
   * - Dễ quản lý lifecycle: tạo khi cần, destroy khi cleanup
   * - Tránh re-render không cần thiết khi props thay đổi
   *
   * Tại sao phải await canplaythrough?
   * - Trình duyệt cần thời gian decode Blob URL thành audio data
   * - Gọi play() trước khi decode xong → NotSupportedError
   * - canplaythrough đảm bảo đủ data để phát liên tục không giật
   * ======================================================================== */
  const togglePlay = useCallback(async () => {
    if (!audioRef.current) {
      /* Convert data URL → Blob URL trước khi tạo Audio element
       * để tránh NotSupportedError trên nhiều trình duyệt */
      const playableUrl = ensureBlobUrl();

      /* Tạo Audio element lần đầu */
      const audio = new Audio();
      audioRef.current = audio;

      /* Khi metadata loaded → lấy duration thực tế */
      audio.onloadedmetadata = () => {
        if (Number.isFinite(audio.duration)) {
          setResolvedDurationMs(Math.round(audio.duration * 1000));
        }
      };

      /* Khi phát xong → reset về trạng thái ban đầu */
      audio.onended = () => {
        setPlaying(false);
        setProgress(0);
        setCurrentTimeMs(0);
        if (animationFrameRef.current) {
          cancelAnimationFrame(animationFrameRef.current);
        }
      };

      /* Xử lý lỗi load/decode – ví dụ codec không hỗ trợ */
      audio.onerror = () => {
        setPlaying(false);
        if (animationFrameRef.current) {
          cancelAnimationFrame(animationFrameRef.current);
        }
      };

      /* Gán src và đợi trình duyệt load + decode xong trước khi phát.
       * Đây là bước quan trọng nhất: nếu gọi play() ngay sau new Audio(url),
       * trình duyệt chưa kịp decode → NotSupportedError */
      audio.src = playableUrl;
      audio.preload = 'auto';

      try {
        await new Promise<void>((resolve, reject) => {
          audio.oncanplaythrough = () => resolve();
          audio.onerror = () => reject(new Error('Không thể phát audio.'));
          /* Timeout 10s phòng trường hợp audio bị treo không load */
          setTimeout(() => resolve(), 10000);
        });
      } catch {
        setPlaying(false);
        audioRef.current = null;
        return;
      }
    }

    const audio = audioRef.current;
    if (!audio) return;

    if (audio.paused) {
      /* Bắt đầu phát + khởi chạy animation loop cho progress bar.
       * Wrap trong try/catch vì play() trả về Promise có thể reject
       * nếu browser chặn autoplay hoặc codec thực sự không hỗ trợ */
      try {
        await audio.play();
        setPlaying(true);
        animationFrameRef.current = requestAnimationFrame(updateProgress);
      } catch {
        /* Nếu play() thất bại → không cập nhật UI thành "playing" */
        setPlaying(false);
      }
    } else {
      /* Tạm dừng + cancel animation loop */
      audio.pause();
      setPlaying(false);
      if (animationFrameRef.current) {
        cancelAnimationFrame(animationFrameRef.current);
      }
    }
  }, [ensureBlobUrl, updateProgress]);

  /* ========================================================================
   * Cleanup effect – khi component unmount:
   * - Dừng audio đang phát
   * - Cancel animation frame
   * - Giải phóng Audio element
   * ======================================================================== */
  useEffect(() => {
    return () => {
      if (audioRef.current) {
        audioRef.current.pause();
        audioRef.current = null;
      }
      if (animationFrameRef.current) {
        cancelAnimationFrame(animationFrameRef.current);
      }
      /* Giải phóng Blob URL – mỗi createObjectURL cấp phát bộ nhớ
       * và giữ tham chiếu tới Blob cho đến khi revokeObjectURL được gọi */
      if (blobUrlRef.current) {
        URL.revokeObjectURL(blobUrlRef.current);
        blobUrlRef.current = null;
      }
    };
  }, []);

  /* ========================================================================
   * RENDER
   *
   * Layout: [Nút Play/Pause tròn] [Waveform + Progress overlay] [Duration]
   *
   * Colors:
   * - isMe (tin nhắn của mình): purple accent
   * - !isMe (tin nhắn đối phương): white/neutral
   *
   * Progress overlay:
   * - Waveform bars phía sau progress line giữ opacity cao (đã phát)
   * - Bars phía trước progress line giữ opacity thấp (chưa phát)
   * - Dùng CSS clip-path hoặc opacity per-bar cho hiệu ứng này
   * ======================================================================== */
  const displayDuration = playing ? currentTimeMs : (resolvedDurationMs || 0);

  /* Màu sắc dựa trên người gửi */
  const barColor = isMe ? 'bg-white/80' : 'bg-[var(--purple-accent)]';
  const barDimColor = isMe ? 'bg-white/30' : 'bg-[var(--purple-accent)]/30';
  const playBtnClass = isMe
    ? 'bg-white/20 hover:bg-white/30 text-white'
    : 'bg-[var(--purple-accent)]/20 hover:bg-[var(--purple-accent)]/30 text-[var(--purple-accent)]';

  return (
    <div className="flex items-center gap-2.5 min-w-[180px] max-w-[280px]">
      {/* Nút Play/Pause tròn */}
      <button
        type="button"
        onClick={() => void togglePlay()}
        className={`w-9 h-9 rounded-full flex items-center justify-center shrink-0 transition-colors ${playBtnClass}`}
        title={playing ? 'Tạm dừng' : 'Phát'}
      >
        {playing ? (
          <Pause className="w-4 h-4" />
        ) : (
          <Play className="w-4 h-4 ml-0.5" />
        )}
      </button>

      {/* ---------------------------------------------------------------
       * Waveform visualization
       *
       * Mỗi bar có 2 lớp opacity:
       * - Nếu bar index / total < progress → đã phát → opacity cao (barColor)
       * - Nếu chưa → opacity thấp (barDimColor)
       * Tạo hiệu ứng "sweep" khi audio đang phát
       * --------------------------------------------------------------- */}
      <div className="flex-1 flex items-center gap-[2px] h-7 min-w-0">
        {barsData.current.map((level, index) => {
          /* Tính xem bar này đã "phát qua" chưa dựa trên progress (0-1) */
          const barProgress = index / STATIC_BAR_COUNT;
          const isPlayed = barProgress <= progress;

          return (
            <div
              key={index}
              className={`flex-1 rounded-full transition-opacity duration-150 ${isPlayed ? barColor : barDimColor}`}
              style={{
                height: `${Math.max(3, Math.round(level * 24))}px`,
              }}
            />
          );
        })}
      </div>

      {/* Duration / current time */}
      <span className={`text-[11px] font-mono tabular-nums shrink-0 ${isMe ? 'text-white/70' : 'text-[var(--text-secondary)]'}`}>
        {formatDuration(displayDuration)}
      </span>
    </div>
  );
}
