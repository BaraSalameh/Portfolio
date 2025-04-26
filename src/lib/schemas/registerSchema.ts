import { z } from 'zod';

export const registerSchema = z
  .object({
    firstname: z.string().min(2, '2 Character at least'),
    lastname: z.string().min(2, '2 Character at least'),
    email: z.string().min(1, 'Email is required'),
    reEmail: z.string().min(1, 'Please confirm your email'),
    password: z.string().min(2, '2 Character at least'),
    rememberMe: z.boolean(),
  })
  .refine((data) => data.email === data.reEmail, {
    path: ['reEmail'],
    message: 'Emails do not match',
  });

export type RegisterFormData = z.infer<typeof registerSchema>;
