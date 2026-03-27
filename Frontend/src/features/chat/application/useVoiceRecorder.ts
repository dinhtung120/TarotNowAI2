'use client';

/**
 * === useVoiceRecorder Hook ===
 *
 * Hook tùy chỉnh quản lý toàn bộ lifecycle ghi âm giọng nói từ microphone.
 *
 * --- Tại sao cần hook riêng? ---
 * Việc ghi âm liên quan đến nhiều Web API phức tạp (getUserMedia, MediaRecorder,
 * AnalyserNode) và cần quản lý lifecycle chặt chẽ (cleanup stream, stop recorder).
 * Tách thành hook riêng giúp:
 * 1. Tái sử dụng ở nhiều nơi nếu cần
 * 2. Dễ test và maintain
 * 3. Tách biệt logic ghi âm khỏi logic hiển thị UI
 *
 * --- Luồng hoạt động ---
 * 1. User gọi startRecording() → xin quyền mic → tạo MediaRecorder (Opus/WebM)
 * 2. Trong khi ghi: AnalyserNode trích xuất biên độ âm thanh mỗi ~80ms
 *    → cập nhật audioLevels[] cho waveform animation
 * 3. User gọi stopRecording() → trả về Blob audio + duration
 * 4. Hoặc cancelRecording() → hủy bỏ, không trả kết quả
 * 5. Tự động dừng sau MAX_DURATION_SECONDS (120s) để tránh file quá lớn
 *
 * --- Lý do chọn Opus/WebM ---
 * Theo Chat_Technical_Design.md: Opus cho chất lượng giọng nói tốt nhất ở bitrate
 * cực thấp (16kbps), file siêu nhỏ giúp gửi nhanh dù mạng chập chờn.
 * WebM là container tiêu chuẩn hỗ trợ Opus trên mọi trình duyệt hiện đại.
 */

import { useCallback, useRef, useState } from 'react';

/* ========================================================================
 * HẰNG SỐ CẤU HÌNH
 * - MAX_DURATION_SECONDS: giới hạn tối đa thời gian ghi để tránh file quá lớn
 *   và tránh user quên tắt mic. 120 giây là cân bằng giữa UX và kích thước file.
 * - ANALYSER_INTERVAL_MS: tần suất lấy mẫu biên độ âm thanh cho waveform.
 *   80ms cho hiệu ứng mượt mà mà không gây lag (khoảng 12.5 fps).
 * - WAVEFORM_BAR_COUNT: số lượng bar hiển thị trên waveform khi đang ghi.
 * ======================================================================== */
const MAX_DURATION_SECONDS = 120;
const ANALYSER_INTERVAL_MS = 80;
const WAVEFORM_BAR_COUNT = 40;

/**
 * Kết quả trả về khi ghi âm hoàn tất thành công.
 * - blob: dữ liệu audio đã nén (Opus/WebM)
 * - durationMs: thời lượng tính bằng millisecond
 */
export interface VoiceRecordingResult {
  blob: Blob;
  durationMs: number;
}

/**
 * Trạng thái ghi âm – giúp UI biết đang ở phase nào
 * - idle: chưa ghi / đã xong
 * - requesting: đang xin quyền microphone
 * - recording: đang ghi âm
 * - error: có lỗi (mic bị từ chối, trình duyệt không hỗ trợ, etc.)
 */
export type RecordingState = 'idle' | 'requesting' | 'recording' | 'error';

export function useVoiceRecorder() {
  /* ========================================================================
   * STATE
   * - recordingState: trạng thái hiện tại của quá trình ghi âm
   * - elapsedMs: thời gian đã ghi (ms), cập nhật mỗi giây cho UI timer
   * - audioLevels: mảng biên độ âm thanh (0-1) cho waveform visualization
   * - errorMessage: thông báo lỗi nếu có
   * ======================================================================== */
  const [recordingState, setRecordingState] = useState<RecordingState>('idle');
  const [elapsedMs, setElapsedMs] = useState(0);
  const [audioLevels, setAudioLevels] = useState<number[]>([]);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  /* ========================================================================
   * REFS – lưu trữ các instance mà KHÔNG gây re-render khi thay đổi
   * Dùng ref thay vì state vì:
   * 1. MediaRecorder/AudioContext cần truy cập nhanh trong callback
   * 2. Thay đổi chúng không cần trigger UI update
   * 3. Tránh stale closure trong các callback bất đồng bộ
   * ======================================================================== */
  const mediaRecorderRef = useRef<MediaRecorder | null>(null);
  const mediaStreamRef = useRef<MediaStream | null>(null);
  const analyserStreamRef = useRef<MediaStream | null>(null);
  const audioContextRef = useRef<AudioContext | null>(null);
  const analyserRef = useRef<AnalyserNode | null>(null);
  const analyserTimerRef = useRef<ReturnType<typeof setInterval> | null>(null);
  const elapsedTimerRef = useRef<ReturnType<typeof setInterval> | null>(null);
  const startTimeRef = useRef<number>(0);
  const chunksRef = useRef<BlobPart[]>([]);
  const cancelledRef = useRef(false);

  /* ========================================================================
   * cleanup – giải phóng TẤT CẢ tài nguyên liên quan đến ghi âm.
   *
   * Tại sao cần cleanup kỹ càng?
   * - MediaStream giữ quyền truy cập mic → nếu không stop, icon mic vẫn
   *   sáng trên browser, user nghĩ app đang nghe lén.
   * - AudioContext chiếm tài nguyên hệ thống → memory leak nếu không close.
   * - Timers nếu không clear → gọi setState trên component đã unmount → warning.
   * ======================================================================== */
  const cleanup = useCallback(() => {
    /* Dừng tất cả timer đang chạy */
    if (analyserTimerRef.current) {
      clearInterval(analyserTimerRef.current);
      analyserTimerRef.current = null;
    }
    if (elapsedTimerRef.current) {
      clearInterval(elapsedTimerRef.current);
      elapsedTimerRef.current = null;
    }

    /* Đóng AudioContext – giải phóng tài nguyên audio system */
    if (audioContextRef.current) {
      audioContextRef.current.close().catch(() => undefined);
      audioContextRef.current = null;
      analyserRef.current = null;
    }

    /* Dừng tracks của stream clone dành cho AnalyserNode */
    if (analyserStreamRef.current) {
      for (const track of analyserStreamRef.current.getTracks()) {
        track.stop();
      }
      analyserStreamRef.current = null;
    }

    /* Dừng tất cả track trong MediaStream – tắt quyền truy cập microphone */
    if (mediaStreamRef.current) {
      for (const track of mediaStreamRef.current.getTracks()) {
        track.stop();
      }
      mediaStreamRef.current = null;
    }

    mediaRecorderRef.current = null;
    chunksRef.current = [];
  }, []);

  /* ========================================================================
   * startRecording – bắt đầu ghi âm từ microphone
   *
   * Luồng xử lý:
   * 1. Kiểm tra trình duyệt hỗ trợ MediaRecorder
   * 2. Xin quyền microphone (getUserMedia)
   * 3. Tạo AudioContext + AnalyserNode cho waveform real-time
   * 4. Tạo MediaRecorder với codec Opus, bitrate 16kbps
   * 5. Bắt đầu ghi + khởi chạy timer đếm thời gian + analyser
   * 6. Tự động dừng sau MAX_DURATION_SECONDS
   *
   * Tại sao dùng audio/webm;codecs=opus?
   * - Opus là codec tối ưu nhất cho giọng nói ở bitrate thấp
   * - WebM là container tiêu chuẩn Web, hỗ trợ trên Chrome/Firefox/Edge
   * - 16kbps đủ rõ ràng cho giọng nói, file ~120KB/phút
   *
   * Fallback: nếu Opus không hỗ trợ → dùng audio/webm generic
   * ======================================================================== */
  const startRecording = useCallback(async () => {
    /* Reset trạng thái trước khi bắt đầu phiên ghi mới */
    setErrorMessage(null);
    cancelledRef.current = false;
    chunksRef.current = [];

    /* Kiểm tra trình duyệt có hỗ trợ API cần thiết không */
    if (typeof navigator === 'undefined' || !navigator.mediaDevices?.getUserMedia) {
      setRecordingState('error');
      setErrorMessage('Trình duyệt không hỗ trợ ghi âm.');
      return;
    }

    setRecordingState('requesting');

    try {
      /* ---------------------------------------------------------------
       * Bước 1: Xin quyền microphone
       * - audio: true = chỉ cần mic, không cần camera
       * - Browser sẽ hiện popup xin quyền nếu chưa cấp
       * - Nếu user từ chối → catch block xử lý lỗi NotAllowedError
       * --------------------------------------------------------------- */
      const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
      mediaStreamRef.current = stream;

      /* Kiểm tra xem user đã cancel trước khi mic được cấp quyền chưa */
      if (cancelledRef.current) {
        for (const track of stream.getTracks()) track.stop();
        setRecordingState('idle');
        return;
      }

      /* ---------------------------------------------------------------
       * Bước 2: Tạo AudioContext + AnalyserNode cho waveform visualization
       *
       * QUAN TRỌNG: Clone stream trước khi dùng cho AnalyserNode!
       *
       * Tại sao phải clone?
       * - MediaRecorder và AnalyserNode cùng đọc audio frames từ stream
       * - Trên một số trình duyệt (đặc biệt Chrome), khi AnalyserNode
       *   consume audio frames, MediaRecorder có thể bị "bỏ lỡ" frames
       *   → gây ra khoảng lặng (silence) trong bản ghi
       * - Clone stream tạo ra bản sao độc lập, mỗi consumer đọc riêng
       *   → không tranh chấp frames, ghi âm liên tục không bị mất tiếng
       *
       * AnalyserNode.fftSize = 256: phân giải tần số vừa phải, đủ cho
       * waveform đẹp mà không tốn nhiều CPU
       * --------------------------------------------------------------- */
      const audioContext = new AudioContext();
      audioContextRef.current = audioContext;

      /* Clone stream riêng cho AnalyserNode – KHÔNG dùng chung với MediaRecorder */
      const analyserStream = stream.clone();
      analyserStreamRef.current = analyserStream;
      const source = audioContext.createMediaStreamSource(analyserStream);
      const analyser = audioContext.createAnalyser();
      analyser.fftSize = 256;
      source.connect(analyser);
      analyserRef.current = analyser;

      /* ---------------------------------------------------------------
       * Bước 3: Tạo MediaRecorder
       *
       * Dùng stream GỐC (không phải clone) cho MediaRecorder.
       * Stream gốc không bị AnalyserNode can thiệp.
       *
       * Chọn mimeType theo thứ tự ưu tiên:
       * 1. audio/webm;codecs=opus → tốt nhất cho giọng nói
       * 2. audio/webm → fallback trên một số browser
       *
       * audioBitsPerSecond = 16000 (16kbps):
       * - Theo Chat_Technical_Design.md: "16kbps, Mono codec" là cấu hình
       *   tối ưu cho voice message
       * - File ~120KB/phút → gửi nhanh dù mạng yếu
       * --------------------------------------------------------------- */
      let mimeType = 'audio/webm;codecs=opus';
      if (!MediaRecorder.isTypeSupported(mimeType)) {
        mimeType = 'audio/webm';
      }
      if (!MediaRecorder.isTypeSupported(mimeType)) {
        cleanup();
        setRecordingState('error');
        setErrorMessage('Trình duyệt không hỗ trợ ghi âm WebM/Opus.');
        return;
      }

      const recorder = new MediaRecorder(stream, {
        mimeType,
        audioBitsPerSecond: 16_000,
      });
      mediaRecorderRef.current = recorder;

      /* ---------------------------------------------------------------
       * Bước 4: Đăng ký event handlers cho MediaRecorder
       *
       * ondataavailable: mỗi khi có chunk dữ liệu → push vào mảng chunks
       * onstop: khi dừng ghi → KHÔNG xử lý ở đây, xử lý trong stopRecording()
       *
       * Tại sao dùng timeslice 250ms trong recorder.start(250)?
       * - Thu dữ liệu mỗi 250ms thay vì đợi stop mới có data
       * - Giúp web worker không bị block quá lâu
       * - Nếu ghi dài, không giữ toàn bộ trong RAM đến cuối
       * --------------------------------------------------------------- */
      recorder.ondataavailable = (event) => {
        if (event.data && event.data.size > 0) {
          chunksRef.current.push(event.data);
        }
      };

      /* Bắt đầu ghi với timeslice 250ms */
      recorder.start(250);
      startTimeRef.current = Date.now();
      setRecordingState('recording');
      setElapsedMs(0);
      setAudioLevels([]);

      /* ---------------------------------------------------------------
       * Bước 5: Timer cập nhật thời gian đã ghi (mỗi 200ms)
       *
       * Cập nhật elapsedMs để UI hiển thị timer mm:ss.
       * 200ms cho hiệu ứng đếm mượt mà hơn 1000ms nhưng không quá tốn CPU.
       *
       * Tự động dừng nếu vượt MAX_DURATION_SECONDS:
       * - Tránh file quá lớn vượt giới hạn 5MB MediaPayload
       * - Tránh user quên tắt mic
       * --------------------------------------------------------------- */
      elapsedTimerRef.current = setInterval(() => {
        const now = Date.now();
        const elapsed = now - startTimeRef.current;
        setElapsedMs(elapsed);

        if (elapsed >= MAX_DURATION_SECONDS * 1000) {
          /* Dừng recorder nếu vượt thời gian tối đa.
           * Không gọi stopRecording() trực tiếp vì nó là async và
           * sẽ được xử lý bởi onstop event handler */
          if (mediaRecorderRef.current?.state === 'recording') {
            mediaRecorderRef.current.stop();
          }
          if (elapsedTimerRef.current) {
            clearInterval(elapsedTimerRef.current);
            elapsedTimerRef.current = null;
          }
        }
      }, 200);

      /* ---------------------------------------------------------------
       * Bước 6: Analyser timer – trích xuất biên độ âm thanh cho waveform
       *
       * getByteFrequencyData() trả về mảng Uint8Array chứa biên độ tần số
       * (0-255) cho mỗi bin tần số.
       *
       * Logic tính waveform bars:
       * 1. Chia mảng tần số thành WAVEFORM_BAR_COUNT nhóm đều nhau
       * 2. Mỗi nhóm lấy giá trị trung bình → 1 bar
       * 3. Normalize về [0, 1] bằng chia cho 255
       *
       * Kết quả: mảng 40 số từ 0-1, UI dùng làm height cho mỗi bar.
       * --------------------------------------------------------------- */
      analyserTimerRef.current = setInterval(() => {
        if (!analyserRef.current) return;

        const dataArray = new Uint8Array(analyserRef.current.frequencyBinCount);
        analyserRef.current.getByteFrequencyData(dataArray);

        /* Chia dataArray thành WAVEFORM_BAR_COUNT nhóm, mỗi nhóm lấy trung bình */
        const step = Math.max(1, Math.floor(dataArray.length / WAVEFORM_BAR_COUNT));
        const bars: number[] = [];
        for (let i = 0; i < WAVEFORM_BAR_COUNT; i++) {
          let sum = 0;
          const start = i * step;
          const end = Math.min(start + step, dataArray.length);
          for (let j = start; j < end; j++) {
            sum += dataArray[j];
          }
          /* Normalize: chia cho 255 (max giá trị byte) × số mẫu trong nhóm */
          bars.push(sum / ((end - start) * 255));
        }

        setAudioLevels(bars);
      }, ANALYSER_INTERVAL_MS);
    } catch (error) {
      /* ---------------------------------------------------------------
       * Xử lý lỗi:
       * - NotAllowedError: user từ chối quyền microphone
       * - NotFoundError: không tìm thấy microphone
       * - Các lỗi khác: hiển thị message generic
       * --------------------------------------------------------------- */
      cleanup();
      setRecordingState('error');

      // Ghi log dưới dạng warn để tránh hiển thị lỗi overlay (red screen) trên Next.js dev server
      // vì đây là lỗi do user từ chối quyền (chuẩn hành vi) chứ không phải bug code
      console.warn('[VoiceRecorder] Lỗi khi xin quyền microphone:', error);

      if (error instanceof DOMException) {
        if (error.name === 'NotAllowedError' || error.name === 'PermissionDeniedError') {
          /* Phân biệt thêm trường hợp không có HTTPS (Secure Context)
           * Mobile browsers thường tự động chặn / ném lỗi nếu truy cập qua IP http://192.x */
          if (window.isSecureContext === false) {
            setErrorMessage('Trình duyệt chặn microphone vì kết nối không bảo mật. Vui lòng thử trên localhost hoặc dùng HTTPS.');
          } else {
            setErrorMessage('Quyền truy cập microphone bị từ chối. Vui lòng cấp quyền trong phần cài đặt trang của trình duyệt và thử lại.');
          }
        } else if (error.name === 'NotFoundError') {
          setErrorMessage('Không tìm thấy microphone trên thiết bị.');
        } else if (error.name === 'NotReadableError' || error.name === 'TrackStartError') {
          setErrorMessage('Microphone đang bị ứng dụng khác sử dụng hoặc gặp sự cố phần cứng.');
        } else {
          setErrorMessage(`Lỗi ghi âm: ${error.message}`);
        }
      } else {
        setErrorMessage('Không thể bắt đầu ghi âm. Vui lòng thử lại.');
      }
    }
  }, [cleanup]);

  /* ========================================================================
   * stopRecording – dừng ghi âm và trả về kết quả
   *
   * Trả về Promise<VoiceRecordingResult | null>:
   * - VoiceRecordingResult nếu ghi thành công (có dữ liệu audio)
   * - null nếu bị cancel hoặc không có dữ liệu
   *
   * Tại sao dùng Promise?
   * MediaRecorder.stop() là bất đồng bộ – event 'stop' fire sau khi
   * tất cả data đã được flush. Cần đợi event này để có đầy đủ chunks.
   * ======================================================================== */
  const stopRecording = useCallback((): Promise<VoiceRecordingResult | null> => {
    return new Promise((resolve) => {
      const recorder = mediaRecorderRef.current;

      /* Nếu không có recorder hoặc đã dừng → trả về null */
      if (!recorder || recorder.state === 'inactive') {
        const durationMs = Date.now() - startTimeRef.current;
        cleanup();
        setRecordingState('idle');
        setElapsedMs(0);
        setAudioLevels([]);

        /* Cố gắng tạo blob từ chunks đã thu được (nếu có) */
        if (chunksRef.current.length > 0) {
          const blob = new Blob(chunksRef.current, { type: 'audio/webm' });
          if (blob.size > 0) {
            resolve({ blob, durationMs: Math.max(1, durationMs) });
            return;
          }
        }
        resolve(null);
        return;
      }

      /* ---------------------------------------------------------------
       * Đăng ký onstop handler TRƯỚC khi gọi stop()
       *
       * Khi MediaRecorder.stop() được gọi:
       * 1. Nó fire một ondataavailable cuối cùng với data còn lại
       * 2. Sau đó fire onstop
       * → Đảm bảo chunksRef có đầy đủ dữ liệu khi onstop chạy
       * --------------------------------------------------------------- */
      recorder.onstop = () => {
        const durationMs = Date.now() - startTimeRef.current;
        const mimeType = recorder.mimeType || 'audio/webm';
        const blob = new Blob(chunksRef.current, { type: mimeType });

        cleanup();
        setRecordingState('idle');
        setElapsedMs(0);
        setAudioLevels([]);

        if (blob.size > 0 && !cancelledRef.current) {
          resolve({ blob, durationMs: Math.max(1, durationMs) });
        } else {
          resolve(null);
        }
      };

      recorder.stop();
    });
  }, [cleanup]);

  /* ========================================================================
   * cancelRecording – hủy bỏ ghi âm, không trả kết quả
   *
   * Dùng khi user bấm nút Hủy (X) trong lúc đang ghi.
   * Set cancelledRef = true để đảm bảo onstop handler không resolve data.
   * ======================================================================== */
  const cancelRecording = useCallback(() => {
    cancelledRef.current = true;

    if (mediaRecorderRef.current && mediaRecorderRef.current.state !== 'inactive') {
      mediaRecorderRef.current.stop();
    }

    cleanup();
    setRecordingState('idle');
    setElapsedMs(0);
    setAudioLevels([]);
    setErrorMessage(null);
  }, [cleanup]);

  /* ========================================================================
   * dismissError – xóa thông báo lỗi, quay về trạng thái idle
   * Dùng khi user bấm dismiss trên error message.
   * ======================================================================== */
  const dismissError = useCallback(() => {
    setRecordingState('idle');
    setErrorMessage(null);
  }, []);

  return {
    recordingState,
    isRecording: recordingState === 'recording',
    isRequesting: recordingState === 'requesting',
    elapsedMs,
    audioLevels,
    errorMessage,
    startRecording,
    stopRecording,
    cancelRecording,
    dismissError,
  };
}
