import { cva, VariantProps } from 'class-variance-authority';

export const main = cva(
    'row-start-2 flex flex-col gap-15 items-center sm:items-start'
);

export type MainVariantProps = VariantProps<typeof main>;
