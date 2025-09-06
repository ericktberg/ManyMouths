import { ArrowLeft, Edit3 } from "lucide-react-native";
import { useEffect, useState } from "react";
import { View, Text } from "react-native";
import { useSafeAreaInsets } from "react-native-safe-area-context";
import { Button } from "@/components/ui/button";

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
  const [recipeImage, setRecipeImage] = useState<string | null>(null);
  const insets = useSafeAreaInsets();

  const getTagColor = (index: number) => {
    const colors = ['tomato', 'lime', 'blueberry', 'grape', 'orange'];
    return colors[index % colors.length];
  };

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

  const instructions = parseInstructions(recipe.instructions);
  const unlinkedIngredients = recipe.ingredients.filter(ing =>
    !['Hoisin sauce', 'Chicken thighs'].includes(ing.name)
  );

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