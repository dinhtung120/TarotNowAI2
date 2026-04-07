import { z } from 'zod';

export const postComposerSchema = z.object({
 content: z.string().trim().min(1).max(5000),
 visibility: z.enum(['public', 'private']),
});

export type PostComposerValues = z.infer<typeof postComposerSchema>;
