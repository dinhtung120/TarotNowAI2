import ReactMarkdown from 'react-markdown';
import remarkGfm from 'remark-gfm';
import { cn } from '@/lib/utils';
import { Star, Sparkles, Wand2 } from 'lucide-react';

/**
 * Component render Markdown cho kết quả trả bài Tarot.
 * Được thiết kế để mang lại cảm giác cao cấp (premium) và huyền bí.
 * 
 * @param {Object} props - Thuộc tính của component.
 * @param {string} props.content - Nội dung Markdown cần render.
 * @param {string} [props.className] - CSS class bổ sung.
 */
export function ReadingMarkdownRenderer({ content, className }: { content: string; className?: string }) {
  return (
    <div className={cn("max-w-none text-left", className)}>
      <ReactMarkdown
        remarkPlugins={[remarkGfm]}
        components={{
          // Tùy chỉnh cách hiển thị tiêu đề (h4 thường dùng cho vị trí lá bài)
          h4: ({ node, ...props }) => (
            <h4
              className="clear-both float-left mr-2 mt-0.5 text-[15px] font-bold tracking-tight text-purple-500 uppercase"
              {...props}
            />
          ),
          // Tùy chỉnh cách hiển thị văn bản in đậm
          strong: ({ node, ...props }) => (
            <strong
              className="font-semibold text-purple-400"
              {...props}
            />
          ),
          // Tùy chỉnh cách hiển thị đoạn văn
          p: ({ node, ...props }) => (
            <p
              className="text-gray-300 leading-relaxed mb-4 text-[15px]"
              {...props}
            />
          ),
          // Tùy chỉnh danh sách
          ul: ({ node, ...props }) => (
            <ul className="list-disc list-inside space-y-2 mb-6 text-gray-300" {...props} />
          ),
          li: ({ node, ...props }) => (
            <li className="leading-relaxed" {...props} />
          ),
          // Tùy chỉnh blockquote
          blockquote: ({ node, ...props }) => (
            <blockquote
              className="border-l-2 border-purple-500/50 pl-4 py-1 my-6 italic text-gray-400"
              {...props}
            />
          ),
          // Tùy chỉnh hr
          hr: ({ node, ...props }) => (
            <hr className="my-8 border-t border-white/10" {...props} />
          )
        }}
      >
        {content}
      </ReactMarkdown>
    </div>
  );
}
