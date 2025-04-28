import { cva, VariantProps } from 'class-variance-authority';

export const subFooter = cva(
    'row-start-3 flex',
    {
        variants: {
            itemsY: {
                center: 'items-center'
            },
            itemsX: {
                center: 'justify-center'
            },
            space: {
                sm: 'gap-5',
                md: 'gap-10',
                lg: 'gap-15'
            },
            paddingX: {
                none: 'px-0',
                sm: 'px-5',
                md: 'px-10',
                lg: 'px-15',
            },
            paddingY: {
                none: 'py-0',
                sm: 'py-5',
                md: 'py-10',
                lg: 'py-15',
            }
        },
        defaultVariants: {
            itemsY: 'center',
            itemsX: 'center',
            space: 'md',
            paddingX: 'md',
            paddingY: 'sm',
        }
    }
);

export type SubFooterVariantProps = VariantProps<typeof subFooter>;
