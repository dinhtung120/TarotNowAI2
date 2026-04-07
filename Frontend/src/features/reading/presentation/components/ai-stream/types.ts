export interface StreamMessage {
 id: string;
 role: 'ai' | 'user';
 content: string;
 isStreaming?: boolean;
}
