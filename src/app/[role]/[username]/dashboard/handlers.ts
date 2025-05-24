'use client';

import { deleteEducation, deleteExperience, deleteProject, educationListQuery, experienceListQuery, projectTechnologyListQuery, sortEducation, sortExperience, sortProject } from "@/lib/apis";
import { useAppDispatch } from "@/lib/store/hooks";
import debounce from "lodash.debounce";
import { useCallback } from "react";

export const useHandleEducationDelete = () => {
  const dispatch = useAppDispatch();

  return async (id: string) => {
        try {
            await dispatch(deleteEducation(id));
            await dispatch(educationListQuery());
        } catch (err) {
            console.error('Failed to delete:', err);
        }
    }
};

export const useHandleProjectDelete = () => {
  const dispatch = useAppDispatch();

  return async (id: string) => {
        try {
            await dispatch(deleteProject(id));
            await dispatch(projectTechnologyListQuery());
        } catch (err) {
            console.error('Failed to delete:', err);
        }
    }
};

export const useHandleExperienceDelete = () => {
  const dispatch = useAppDispatch();

  return async (id: string) => {
        try {
            await dispatch(deleteExperience(id));
            await dispatch(experienceListQuery());
        } catch (err) {
            console.error('Failed to delete:', err);
        }
    }
};

export const useDebouncedSortProject = () => {
  const dispatch = useAppDispatch();

  return useCallback(
        debounce(async (lstIds: string[]) => {
            if (lstIds.length > 0) {
                await dispatch(sortProject(lstIds));
                await dispatch(projectTechnologyListQuery());
            }
        }, 1000),
        [dispatch]
    );
};

export const useDebouncedSortEducation = () => {
  const dispatch = useAppDispatch();

  return useCallback(
        debounce(async (lstIds: string[]) => {
            if (lstIds.length > 0) {
                await dispatch(sortEducation(lstIds));
                await dispatch(educationListQuery());
            }
        }, 1000),
        [dispatch]
    );
};

export const useDebouncedSortExperience = () => {
  const dispatch = useAppDispatch();

  return useCallback(
        debounce(async (lstIds: string[]) => {
            if (lstIds.length > 0) {
                await dispatch(sortExperience(lstIds));
                await dispatch(experienceListQuery());
            }
        }, 1000),
        [dispatch]
    );
};