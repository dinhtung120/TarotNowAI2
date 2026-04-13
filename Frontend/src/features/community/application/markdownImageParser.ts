export interface MarkdownTextSegment {
  kind: 'text';
  value: string;
}

export interface MarkdownImageSegment {
  alt: string;
  kind: 'image';
  url: string;
}

export type MarkdownSegment = MarkdownTextSegment | MarkdownImageSegment;

const IMAGE_REGEX = /!\[(.*?)\]\((.*?)\)/g;

export function parseMarkdownSegments(content: string): MarkdownSegment[] {
  const segments: MarkdownSegment[] = [];
  let cursor = 0;

  for (const match of content.matchAll(IMAGE_REGEX)) {
    const start = match.index ?? 0;
    if (start > cursor) {
      segments.push({ kind: 'text', value: content.slice(cursor, start) });
    }

    const alt = match[1] ?? 'community-image';
    const url = match[2] ?? '';
    segments.push({ kind: 'image', alt, url });

    cursor = start + match[0].length;
  }

  if (cursor < content.length) {
    segments.push({ kind: 'text', value: content.slice(cursor) });
  }

  return segments;
}

export function isRenderableImageUrl(url: string): boolean {
  return url.startsWith('http') || url.startsWith('/');
}
