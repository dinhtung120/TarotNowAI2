import type { CombinedProps, TextareaProps } from '@/shared/ui/input/input.types';

export function getTextareaDomProps(props: CombinedProps) {
  const textareaProps = props as TextareaProps;
  const { isTextarea, ...textareaDomProps } = textareaProps;
  void isTextarea;
  return textareaDomProps;
}
