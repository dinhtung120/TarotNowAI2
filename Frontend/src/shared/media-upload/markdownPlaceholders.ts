export interface MarkdownImagePlaceholder {
  altText: string;
  id: string;
  tokenUrl: string;
}

const PLACEHOLDER_SCHEME = 'tarotnow-upload://';

export function createMarkdownImagePlaceholder(altText: string): MarkdownImagePlaceholder {
  const id = typeof crypto !== 'undefined' && typeof crypto.randomUUID === 'function'
    ? crypto.randomUUID()
    : `${Date.now()}-${Math.random().toString(16).slice(2)}`;

  return {
    id,
    altText,
    tokenUrl: `${PLACEHOLDER_SCHEME}${id}`,
  };
}

export function appendPlaceholderMarkdown(content: string, placeholder: MarkdownImagePlaceholder): string {
  const markdown = toImageMarkdown(placeholder.altText, placeholder.tokenUrl);
  const trimmed = content.trimEnd();
  return trimmed ? `${trimmed}\n\n${markdown}\n` : `${markdown}\n`;
}

export function replacePlaceholderMarkdown(
  content: string,
  placeholder: MarkdownImagePlaceholder,
  imageUrl: string,
  altText = 'community-image',
): string {
  const source = toImageMarkdown(placeholder.altText, placeholder.tokenUrl);
  const destination = toImageMarkdown(altText, imageUrl);
  if (content.includes(source)) {
    return content.replace(source, destination);
  }

  return appendRealImageMarkdown(content, imageUrl, altText);
}

export function removePlaceholderMarkdown(content: string, placeholder: MarkdownImagePlaceholder): string {
  const source = toImageMarkdown(placeholder.altText, placeholder.tokenUrl);
  if (!content.includes(source)) {
    return content;
  }

  return normalizeMarkdown(content.replace(source, '').replace(/\n{3,}/g, '\n\n'));
}

export function appendRealImageMarkdown(content: string, imageUrl: string, altText = 'community-image'): string {
  const trimmed = content.trimEnd();
  const markdown = toImageMarkdown(altText, imageUrl);
  return trimmed ? `${trimmed}\n\n${markdown}\n` : `${markdown}\n`;
}

function toImageMarkdown(altText: string, url: string): string {
  return `![${altText}](${url})`;
}

function normalizeMarkdown(content: string): string {
  return content.trimEnd();
}
