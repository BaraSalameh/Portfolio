import { z } from 'zod';

export const loginSchema = z.object({
    email: z.string().min(1, 'Email is required'), // Accepts any non-empty string
    password: z.string().min(2, '2 Character at least'),
    rememberMe: z.boolean()
});

export type LoginFormData = z.infer<typeof loginSchema>;
