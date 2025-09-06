import { ArrowLeft, Edit3 } from "lucide-react-native";
import { useEffect, useState } from "react";
import { View, Text } from "react-native";
import { useSafeAreaInsets } from "react-native-safe-area-context";
import { Button } from "@/components/ui/button";
import { RecipeDetailsModel } from "@/src/domain-models/recipe-models";

interface RecipeOverviewProps {
  recipe: RecipeDetailsModel;
  onBack: () => void;
  onEdit: () => void;
  onRate: () => void;
  onAddToMeal: () => void;
  onGoToMeal: () => void;
  onStartCooking: () => void;
  onLinkIngredients: () => void;
  isInMeal: boolean;
}

export function RecipeOverview({
  recipe,
  onBack,
  onEdit,
  onRate,
  onAddToMeal,
  onGoToMeal,
  onStartCooking,
  onLinkIngredients,
  isInMeal
}: RecipeOverviewProps) {

  const parseInstructions = (instructions: string) => {
    return instructions.split('\n').filter(line => line.trim()).map((step, index) => {
      const stepMatch = step.match(/^(\d+)\.\s*(.*)/);
      if (stepMatch) {
        return {
          number: parseInt(stepMatch[1]),
          text: stepMatch[2],
          hasTimer: /\[\d{2}:\d{2}\]/.test(step),
          hasTemperature: /\d+°[FC]/.test(step),
          hasIngredient: /\[[^\]]+\]/.test(step) && !/\[\d{2}:\d{2}\]/.test(step)
        };
      }
      return null;
    }).filter(Boolean);
  };
  
  return (
    <View className='flex justify-center align-middle'>
      <HeaderControls recipe={recipe} />
      <Text>{recipe.description}</Text>
    </View>
  )
}

interface HeaderControlsProps {
  recipe: RecipeDetailsModel;
}

function HeaderControls({ recipe }: HeaderControlsProps) {
  /* Header Controls
       - Back Button (integrated in view)
       - Edit button
       - Favorite button

      This is a single bar that places them in the appropriate locations
     */
  return (
    <View className='flex flex-row p-2 justify-between items-start'>
      <Button
        variant="secondary"
        size="sm"
        className="transparent backdrop-blur-sm hover:bg-orange"
      >
        <ArrowLeft className="h-4 w-4" />
      </Button>
      <View className='flex flex-row gap-2 justify-right'>
        <Button
          variant="secondary"
          size="sm"
          className="transparent backdrop-blur-sm hover:bg-orange"
        >
          <Edit3 className="h-4 w-4" />
        </Button>
      </View>
    </View>
  );
}