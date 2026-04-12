/// <reference lib="webworker" />

type WorkerInMessage = {
  id: string;
  buffer: ArrayBuffer;
  mimeType: string;
  maxEdge: number;
  quality: number;
};

self.onmessage = async (ev: MessageEvent<WorkerInMessage>) => {
  const { id, buffer, mimeType, maxEdge, quality } = ev.data;
  try {
    const blob = new Blob([buffer], { type: mimeType || 'image/jpeg' });
    const bmp = await createImageBitmap(blob);
    const ratio = Math.min(1, maxEdge / Math.max(bmp.width, bmp.height));
    const tw = Math.max(1, Math.round(bmp.width * ratio));
    const th = Math.max(1, Math.round(bmp.height * ratio));
    const canvas = new OffscreenCanvas(tw, th);
    const ctx = canvas.getContext('2d');
    if (!ctx) {
      bmp.close();
      throw new Error('Không khởi tạo được canvas.');
    }

    ctx.drawImage(bmp, 0, 0, tw, th);
    bmp.close();
    const out = await canvas.convertToBlob({ type: 'image/webp', quality });
    if (!out) {
      throw new Error('Không nén được WebP.');
    }

    const outBuf = await out.arrayBuffer();
    self.postMessage({ id, buffer: outBuf }, [outBuf]);
  } catch (e) {
    const message = e instanceof Error ? e.message : String(e);
    self.postMessage({ id, error: message });
  }
};

export {};
