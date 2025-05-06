import { cva, VariantProps } from 'class-variance-authority';

export const header = cva(
    'flex row-start-1',
    {
        variants: {
            itemsY: {
                center: 'sm:items-center'
            },
            itemsX: {
                center: 'justify-center',
                start: 'justify-start',
                between: 'justify-between'
            },
            space: {
                none: 'gap-0',
                sm: 'gap-5',
                md: 'gap-10',
                lg: 'gap-15'
            },
            paddingX: {
                none: 'px-0',
                xs: 'px-3',
                sm: 'px-5',
                md: 'px-10',
                lg: 'px-15',
            },
            paddingY: {
                none: 'py-0',
                xs: 'py-3',
                sm: 'py-5',
                md: 'py-10',
                lg: 'py-15',
            }
        },
        defaultVariants: {
            itemsY: 'center',
            itemsX: 'start',
            space: 'md',
            paddingX: 'md',
            paddingY: 'sm',
        }
    }
);

export type HeaderVariantProps = VariantProps<typeof header>;
