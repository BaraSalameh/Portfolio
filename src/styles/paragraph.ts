import { cva, VariantProps } from 'class-variance-authority';

export const paragraph = cva(
    'font-[family-name:var(--font-geist-mono)]',
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
                sm: 'text-xs sm:text-xs',
                md: 'text-xs sm:text-md',
                lg: 'text-sm sm:text-lg',
                xl: 'text-lg sm:text-xl'
            },
            text: {
                standard: '',
                justify: 'text-justify',
            }
        },
        defaultVariants: {
            intent: 'standard',
            size: 'md',
            text: 'standard'
        },
    }
);

export type ParagraphVariantProps = VariantProps<typeof paragraph>;
