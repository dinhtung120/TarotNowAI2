export const MAX_DURATION_SECONDS = 120;
export const ANALYSER_INTERVAL_MS = 80;
export const WAVEFORM_BAR_COUNT = 40;

export function stopMediaStream(stream: MediaStream | null) {
 if (!stream) return;
 for (const track of stream.getTracks()) track.stop();
}

export function resolveRecorderMimeType() {
 const preferred = 'audio/webm;codecs=opus';
 if (MediaRecorder.isTypeSupported(preferred)) return preferred;
 if (MediaRecorder.isTypeSupported('audio/webm')) return 'audio/webm';
 return null;
}

export function buildWaveformBars(dataArray: Uint8Array, barCount: number) {
 const step = Math.max(1, Math.floor(dataArray.length / barCount));
 const bars: number[] = [];

 for (let i = 0; i < barCount; i++) {
  let sum = 0;
  const start = i * step;
  const end = Math.min(start + step, dataArray.length);
  for (let j = start; j < end; j++) sum += dataArray[j];
  bars.push(sum / ((end - start) * 255));
 }

 return bars;
}

export function mapVoiceRecorderError(error: unknown) {
 if (!(error instanceof DOMException)) {
  return 'Không thể bắt đầu ghi âm. Vui lòng thử lại.';
 }

 if (error.name === 'NotAllowedError' || error.name === 'PermissionDeniedError') {
  if (window.isSecureContext === false) {
   return 'Trình duyệt chặn microphone vì kết nối không bảo mật. Vui lòng thử trên localhost hoặc dùng HTTPS.';
  }
  return 'Quyền truy cập microphone bị từ chối. Vui lòng cấp quyền trong phần cài đặt trang của trình duyệt và thử lại.';
 }

 if (error.name === 'NotFoundError') {
  return 'Không tìm thấy microphone trên thiết bị.';
 }

 if (error.name === 'NotReadableError' || error.name === 'TrackStartError') {
  return 'Microphone đang bị ứng dụng khác sử dụng hoặc gặp sự cố phần cứng.';
 }

 return `Lỗi ghi âm: ${error.message}`;
}
