import { z } from 'zod';

const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

export const contactMessageSchema = z.object({
    emailTo: z
        .string()
        .min(1, "Receiver's email is required")
        .regex(emailRegex, "Receiver's email not valid"),

    name: z
        .string()
        .optional()
        .nullable(),

    email: z
        .string()
        .min(1, "Your email is required")
        .regex(emailRegex, "Email isn't valid"),

    subject: z
        .string()
        .min(1, "Subject is required")
        .max(20, "Subject can't be too long"),

    message: z.string()
        .min(10, "Message is too short")
        .max(1000, 'Message is too long'),
})

export type ContactMessageFormData = z.infer<typeof contactMessageSchema>;