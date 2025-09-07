import { ArrowLeft, Edit3 } from "lucide-react-native";
import React, { useState, useEffect } from "react";
import { View, Text } from "react-native";
import { useSafeAreaInsets } from "react-native-safe-area-context";
import { Button } from "@/components/ui/button";
import { RecipeDetailsModel } from "@/src/domain-models/recipe-models";
import { router } from "expo-router";
import { IngredientBaseModel } from "@/src/domain-models/ingredient-models";
import Modal from "react-native-modal";

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
  handleLinkIngredientClick: (ingredient: IngredientBaseModel) => void;
  onLinkIngredient: () => Promise<void>;
  fetchIngredientInfo: () => Promise<any>;
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
  isInMeal,
  handleLinkIngredientClick,
  onLinkIngredient,
  fetchIngredientInfo
}: RecipeOverviewProps) {
  const [modalVisible, setModalVisible] = useState(false);
  const [selectedIngredient, setSelectedIngredient] = useState<IngredientBaseModel | null>(null);
  const [ingredientInfo, setIngredientInfo] = useState<any>(null);

  // For now, mock linked status for demo. In real app, this would come from API/model.
  const getLinkedStatus = (ingredient: any) => {
    // Demo: first 2 linked, rest not
    const idx = recipe.ingredients.indexOf(ingredient);
    if (idx < 2) return 'linked';
    return 'needs-linking';
  };

  async function handleIngredientClick(ingredient: IngredientBaseModel) {
    setSelectedIngredient(ingredient);
    setModalVisible(true);
    // Fetch extra info if needed
    const info = await fetchIngredientInfo();
    setIngredientInfo(info);
  }

  async function handleLink() {
    if (selectedIngredient) {
      await onLinkIngredient();
      setModalVisible(false);
      setSelectedIngredient(null);
      setIngredientInfo(null);
    }
  }

  return (
    <View className=" bg-white">
      {/* Header Controls */}
      <HeaderControls recipe={recipe} />

      {/* Title Card */}
      <View className="border-2 border-orange-500  rounded-2xl shadow-md mx-4 mt-2 p-4 flex-row items-center justify-between">
        <View style={{flex: 1}}>
          <Text className="text-2xl font-bold text-gray-900 mb-1">{recipe.name}</Text>
          <View className="flex-row items-center mb-1">
            <Text className="text-xs text-gray-500 mr-2">• Cooked 8x</Text>
            <Text className="text-xs text-yellow-500">★ 4.6</Text>
          </View>
        </View>
        <View className="flex-row items-center gap-2">
          <Text className="bg-green-200 text-green-700 font-bold px-3 py-1 rounded-full text-sm ml-2">$12.50</Text>
        </View>
      </View>

      {/* Meta Info Row */}
      <View className="flex-row justify-around mt-4 mb-2 mx-4">
        <View className="items-center flex-1">
          <Text className="text-orange-500 text-lg font-bold">Prep</Text>
          <Text className="text-xl font-bold text-gray-900">{recipe.prepTimeMinutes}m</Text>
        </View>
        <View className="items-center flex-1">
          <Text className="text-yellow-600 text-lg font-bold">Cook</Text>
          <Text className="text-xl font-bold text-gray-900">{recipe.cookTimeMinutes}m</Text>
        </View>
        <View className="items-center flex-1">
          <Text className="text-blue-600 text-lg font-bold">Serves</Text>
          <Text className="text-xl font-bold text-gray-900">{recipe.servings}</Text>
        </View>
      </View>

      {/* Description */}
      <Text className="text-gray-600 text-base mx-6 mt-2 mb-2">{recipe.description}</Text>

      {/* Tags */}
      <View className="flex-row flex-wrap gap-2 mx-6 mb-2">
        <Text className="bg-red-100 text-red-500 px-2 py-1 rounded-full text-xs">Quick</Text>
        <Text className="bg-green-100 text-green-500 px-2 py-1 rounded-full text-xs">Asian</Text>
        <Text className="bg-blue-100 text-blue-500 px-2 py-1 rounded-full text-xs">Protein</Text>
        <Text className="bg-purple-100 text-purple-500 px-2 py-1 rounded-full text-xs">Family-Friendly</Text>
      </View>

      {/* Action Buttons */}
      <View className="mx-4 mt-2 mb-2">
        <View className="flex-row items-center mb-2">
          <View className="flex-1 bg-orange-400 rounded-lg mr-2">
            <Text className="text-white text-center py-2 font-bold text-base">+ Add to Meal</Text>
          </View>
        </View>
        <View className="flex-row gap-2 mb-2">
          <View className="flex-1 border border-gray-300 rounded-lg mr-2">
            <Text className="text-center py-2 font-semibold text-gray-700">▶ Start Cooking</Text>
          </View>
          <View className="flex-1 border border-gray-300 rounded-lg">
            <Text className="text-center py-2 font-semibold text-gray-700">♡ Rate Recipe</Text>
          </View>
        </View>
        <View className="flex-row items-center justify-center mb-2">
          <Text className="text-gray-600 text-base">🔗 Link Ingredients (6 unlinked)</Text>
        </View>
      </View>

      {/* Ingredients Card */}
      <View className="bg-white border border-gray-200 rounded-2xl shadow-sm mx-4 mb-6 p-4">
        <View className="flex-row items-center mb-2">
          <Text className="text-lg font-bold text-green-700 mr-2">🛒 Ingredients</Text>
        </View>
        {/* Ingredient List */}
        {recipe.ingredients.map((ingredient, idx) => {
          const linked = getLinkedStatus(ingredient) === 'linked';
          return (
            <View key={ingredient.id || idx} className="flex-row items-center justify-between mb-1">
              <View className="flex-row items-center">
                <Text className="text-base text-gray-900 mr-2">{ingredient.name}</Text>
                {linked ? (
                  <Text className="bg-green-100 text-green-600 px-2 py-0.5 rounded-full text-xs ml-1">✓ Linked</Text>
                ) : null}
              </View>
              <View className="flex-row items-center gap-2">
                <Text className="text-base text-gray-700">{ingredient.amount} {ingredient.unit}</Text>
                {!linked && (
                  <Button
                    variant="outline"
                    size="sm"
                    onPress={() => handleIngredientClick(ingredient)}
                  >
                    Link
                  </Button>
                )}
              </View>
            </View>
          );
        })}
      </View>

      {/* Link Ingredient Modal */}
      <Modal isVisible={modalVisible} onBackdropPress={() => setModalVisible(false)}>
        <View className="bg-white p-4 rounded-lg shadow-lg">
          <Text className="text-lg font-bold mb-4">Link Ingredient</Text>
          {ingredientInfo && (
            <View>
              <Text className="text-gray-800 mb-2">Ingredient: {ingredientInfo.name}</Text>
              {/* Add more ingredient details here as needed */}
            </View>
          )}
          <Button
            onPress={() => handleLink(/* linkData */)}
            className="bg-blue-600 rounded-lg py-2"
          >
            Does Nothing
          </Button>
        </View>
      </Modal>
    </View>
  );
}

interface HeaderControlsProps {
  recipe: RecipeDetailsModel;
}

function HeaderControls({ recipe }: HeaderControlsProps) {
  // Always go to the recipes list
  const goToRecipesList = () => {
    router.replace('/recipes');
  };
  return (
    <View className='flex flex-row p-2 justify-between items-start'>
      <Button
        variant="secondary"
        size="sm"
        className="transparent backdrop-blur-sm hover:bg-orange"
        onPress={goToRecipesList}
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