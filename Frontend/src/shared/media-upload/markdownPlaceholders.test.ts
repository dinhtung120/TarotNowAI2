import { describe, expect, it } from 'vitest';
import {
  appendPlaceholderMarkdown,
  createMarkdownImagePlaceholder,
  removePlaceholderMarkdown,
  replacePlaceholderMarkdown,
} from '@/shared/media-upload';

describe('markdownPlaceholders', () => {
  it('appends and replaces placeholder markdown', () => {
    const placeholder = createMarkdownImagePlaceholder('uploading-image');
    const withPlaceholder = appendPlaceholderMarkdown('Hello world', placeholder);

    expect(withPlaceholder).toContain(`![uploading-image](${placeholder.tokenUrl})`);

    const withRealImage = replacePlaceholderMarkdown(
      withPlaceholder,
      placeholder,
      'https://media.example.com/community/image.webp',
      'community-image',
    );

    expect(withRealImage).toContain('![community-image](https://media.example.com/community/image.webp)');
    expect(withRealImage).not.toContain(placeholder.tokenUrl);
  });

  it('removes placeholder markdown for rollback', () => {
    const placeholder = createMarkdownImagePlaceholder('uploading-image');
    const withPlaceholder = appendPlaceholderMarkdown('content', placeholder);

    const rolledBack = removePlaceholderMarkdown(withPlaceholder, placeholder);

    expect(rolledBack).toBe('content');
  });
});
