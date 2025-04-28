import { cva, type VariantProps } from 'class-variance-authority';

export const main = cva(
    'row-start-2 flex',
    {
        variants: {
        direction: {
            row: 'flex-row',
            col: 'flex-col',
            wrap: 'flex-wrap'
        },
        itemsX: {
            center: '',
            start: '',
        },
        itemsY: {
            center: '',
            start: '',
        },
        space: {
            sm: 'gap-5',
            md: 'gap-10',
            lg: 'gap-15',
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
        },
        },
        compoundVariants: [
            // Row direction
            { direction: 'row', itemsX: 'center', className: 'justify-center' },
            { direction: 'row', itemsX: 'start',  className: 'justify-start' },
            { direction: 'row', itemsY: 'center', className: 'items-center' },
            { direction: 'row', itemsY: 'start',  className: 'items-start' },

            // Column direction
            { direction: 'col', itemsX: 'center', className: 'items-center' },
            { direction: 'col', itemsX: 'start',  className: 'items-start' },
            { direction: 'col', itemsY: 'center', className: 'justify-center' },
            { direction: 'col', itemsY: 'start',  className: 'justify-start' },
        ],
            defaultVariants: {
            direction: 'col',
            itemsX: 'center',
            itemsY: 'center',
            space: 'md',
            paddingX: 'md',
            paddingY: 'sm',
        },
    }
);

export type MainVariantProps = VariantProps<typeof main>;
