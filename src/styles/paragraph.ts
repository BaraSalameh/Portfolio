import { cva, VariantProps } from 'class-variance-authority';

export const paragraph = cva(
    'text-justify font-[family-name:var(--font-geist-mono)]',
    {
        variants: {
            intent: {
                primary:
                    'text-green-900 dark:text-green-900',
                secondary:
                    'text-gray-900 dark:text-gray-100',
                danger:
                    'text-red-700 dark:text-red-400',
                standard:
                    '',
            },
            size: {
                xs: 'text-xs/6',
                sm: 'text-sm/6',
                md: 'text-md/6',
                lg: 'text-lg/6',
                xl: 'text-2xl/6'
            }
        },
        defaultVariants: {
            intent: 'standard',
            size: 'md'
        },
    }
);

export type ParagraphVariantProps = VariantProps<typeof paragraph>;
