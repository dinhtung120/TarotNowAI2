import ReactMarkdown from 'react-markdown';
import remarkGfm from 'remark-gfm';
import { cn } from '@/lib/utils';

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
          h4: (props) => (
            <span
              className="text-[15px] font-bold tracking-tight text-purple-400 after:content-[':'] mr-0.5"
              {...props}
            />
          ),
          // Tùy chỉnh cách hiển thị văn bản in đậm
          strong: (props) => (
            <strong
              className="font-semibold text-purple-400"
              {...props}
            />
          ),
          // Tùy chỉnh cách hiển thị đoạn văn
          p: (props) => (
            <span
              className="text-gray-300 leading-relaxed text-[15px] inline"
            >
              {props.children}
              <span className="block h-4" />
            </span>
          ),
          // Tùy chỉnh danh sách
          ul: (props) => (
            <ul className="list-disc list-inside space-y-2 mb-6 text-gray-300" {...props} />
          ),
          li: (props) => (
            <li className="leading-relaxed" {...props} />
          ),
          // Tùy chỉnh blockquote
          blockquote: (props) => (
            <blockquote
              className="border-l-2 border-purple-500/50 pl-4 py-1 my-6 italic text-gray-400"
              {...props}
            />
          ),
          // Tùy chỉnh hr
          hr: (props) => (
            <hr className="my-8 border-t border-white/10" {...props} />
          )
        }}
      >
        {content}
      </ReactMarkdown>
    </div>
  );
}
