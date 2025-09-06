import React, { useEffect, useState } from 'react';
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  ScrollView,
  SafeAreaView,
  Alert,
} from 'react-native';

import {
  ArrowLeft,
  Plus,
  Trash2,
  BookOpen,
  ChefHat,
  StickyNote,
  Clock,
  Users,
  Timer,
  ReplyIcon
} from 'lucide-react-native';
import DropDownPicker, { ItemType, ValueType } from 'react-native-dropdown-picker';
import { produce } from 'immer';
import { RecipeInputModel, IngredientInputModel } from '@/src/domain-models/recipe-models'


interface RecipeCreationProps {
  onBack: () => void;
  onSave: (recipe: RecipeInputModel) => void;
}

interface RecipeSubMenuProps {
  recipeInfo: RecipeInputModel,
  setRecipeInfo: React.Dispatch<React.SetStateAction<RecipeInputModel>>;
}

export function RecipeCreationForm({ onBack, onSave }: RecipeCreationProps) {
  // Local effects
  const [activeTab, setActiveTab] = useState<'info' | 'ingredients' | 'instructions'>('info');

  const [recipeInfo, setRecipeInfo] = useState<RecipeInputModel>({
    name: '',
    description: '',
    instructions: '',
    servings: 0,
    prepTimeMinutes: 0,
    cookTimeMinutes: 0,
    ingredients: []
  });

  const handleSave = () => {
    onSave(recipeInfo);
  };

  const canSave = recipeInfo.name && recipeInfo.ingredients.length > 0 && recipeInfo.instructions;

  return (
    <SafeAreaView className="flex-1 bg-gray-50">
      <Text />
      {/* Header */}
      <View className="flex-row items-center justify-between px-4 py-3 bg-white border-b border-gray-200">
        <TouchableOpacity className="p-2" onPress={onBack}>
          <ArrowLeft size={20} className="text-gray-600" />
        </TouchableOpacity>
        <View className="flex-1 items-center">
          <Text className="text-2xl font-bold bg-gradient-to-r from-red-500 to-orange-500 text-red-500">
            Create Recipe
          </Text>
          <Text className="text-xs text-gray-500 mt-0.5">Build your culinary masterpiece</Text>
        </View>
        <TouchableOpacity
          className={`px-4 py-2 rounded-lg shadow-lg ${canSave
            ? 'bg-gradient-to-r from-red-500 to-orange-500 bg-red-500'
            : 'bg-gray-300'
            }`}
          onPress={handleSave}
          disabled={!canSave}
        >
          <Text className="text-white font-semibold text-sm">Save Recipe</Text>
        </TouchableOpacity>
      </View>

      {/* Tabs */}
      <View className="flex-row bg-gray-100/50 p-1">
        <TouchableOpacity
          className={`flex-1 py-4 items-center rounded-lg ${activeTab === 'info'
            ? 'bg-red-500 shadow-sm'
            : 'bg-transparent'
            }`}
          onPress={() => setActiveTab('info')}
        >
          <View className="flex-row items-center">
            <BookOpen size={16} className={activeTab === 'info' ? 'text-white' : 'text-gray-600'} />
            <Text className={`ml-2 font-medium ${activeTab === 'info'
              ? 'text-white'
              : 'text-gray-600'
              }`}>
              Info
            </Text>
          </View>
        </TouchableOpacity>

        <TouchableOpacity
          className={`flex-1 py-4 items-center rounded-lg ${activeTab === 'ingredients'
            ? 'bg-red-500 shadow-sm'
            : 'bg-transparent'
            }`}
          onPress={() => setActiveTab('ingredients')}
        >
          <View className="flex-row items-center">
            <ChefHat size={16} className={activeTab === 'ingredients' ? 'text-white' : 'text-gray-600'} />
            <Text className={`ml-2 font-medium ${activeTab === 'ingredients'
              ? 'text-white'
              : 'text-gray-600'
              }`}>
              Ingredients
            </Text>
          </View>
        </TouchableOpacity>

        <TouchableOpacity
          className={`flex-1 py-4 items-center rounded-lg ${activeTab === 'instructions'
            ? 'bg-red-500 shadow-sm'
            : 'bg-transparent'
            }`}
          onPress={() => setActiveTab('instructions')}
        >
          <View className="flex-row items-center">
            <StickyNote size={16} className={activeTab === 'instructions' ? 'text-white' : 'text-gray-600'} />
            <Text className={`ml-2 font-medium ${activeTab === 'instructions'
              ? 'text-white'
              : 'text-gray-600'
              }`}>
              Instructions
            </Text>
          </View>
        </TouchableOpacity>
      </View>

      {/* Tab Content */}
      <ScrollView>
      <View className="flex-1">
        <View style={{ display: activeTab === 'info' ? "flex" : "none" }}>
          <InformationTab recipeInfo={recipeInfo} setRecipeInfo={setRecipeInfo} />
        </View>
        <View style={{ display: activeTab === 'ingredients' ? "flex" : "none" }}>
          <IngredientsTab recipeInfo={recipeInfo} setRecipeInfo={setRecipeInfo}
          />
        </View>
        <View style={{ display: activeTab === 'instructions' ? "flex" : "none" }}>
          <InstructionsTab recipeInfo={recipeInfo} setRecipeInfo={setRecipeInfo}
          />
        </View>
      </View>
      </ScrollView>
    </SafeAreaView>
  );
}

/**
 * 
 * @returns 
 */
function InformationTab({ recipeInfo, setRecipeInfo }: RecipeSubMenuProps) {
  return (
    <ScrollView className="flex-1 p-4 pb-20">
      {/* Basic Info Card */}
      <View className="bg-gradient-to-br from-blue-50 to-purple-50 border-2 border-blue-200 rounded-xl mb-6 shadow-sm">
        <View className="p-4 border-b border-blue-100">
          <View className="flex-row items-center">
            <BookOpen size={20} className="text-blue-600 mr-2" />
            <Text className="text-lg font-bold text-gray-800">Basic Information</Text>
          </View>
        </View>
        <View className="p-4 space-y-4">
          <View>
            <Text className="text-sm font-semibold text-gray-700 mb-1">Recipe Name</Text>
            <TextInput
              className="bg-white border-2 border-gray-200 focus:border-blue-400 rounded-lg px-3 py-2 text-base"
              placeholder="Enter recipe name"
              value={recipeInfo.name}
              onChangeText={(text) => setRecipeInfo((prev) => produce(prev, draft => {
                draft.name = text;
              }))}
            />
          </View>

          <View>
            <Text className="text-sm font-semibold text-gray-700 mb-1">Description</Text>
            <TextInput
              className="bg-white border-2 border-gray-200 focus:border-blue-400 rounded-lg px-3 py-2 text-base min-h-20"
              placeholder="Brief description of the recipe"
              value={recipeInfo.description}
              onChangeText={(text) => setRecipeInfo(prev => produce(prev, draft => {
                draft.description = text;
              }))}
              multiline
              numberOfLines={3}
              textAlignVertical="top"
            />
          </View>
        </View>
      </View>

      {/* Timing & Servings Card */}
      <View className="bg-gradient-to-br from-orange-50 to-yellow-50 border-2 border-orange-200 rounded-xl mb-6 shadow-sm">
        <View className="p-4 border-b border-orange-100">
          <View className="flex-row items-center">
            <Clock size={20} className="text-orange-600 mr-2" />
            <Text className="text-lg font-bold text-gray-800">Timing & Servings</Text>
          </View>
        </View>
        <View className="p-4">
          <View className="flex-row space-x-4">
            <View className="flex-1">
              <View className="flex-row items-center mb-1">
                <Users size={16} className="text-blue-600 mr-1" />
                <Text className="text-sm font-semibold text-gray-700">Servings</Text>
              </View>
              <TextInput
                className="bg-white border-2 border-gray-200 focus:border-orange-400 rounded-lg px-3 py-2 text-base"
                placeholder="4"
                value={recipeInfo.servings.toString()}
                onChangeText={(text) => setRecipeInfo(prev => produce(prev, draft => {
                  draft.servings = parseInt(text) || 1;
                }))}
                keyboardType="numeric"
              />
            </View>
            <View className="flex-1">
              <View className="flex-row items-center mb-1">
                <ChefHat size={16} className="text-lime-600 mr-1" />
                <Text className="text-sm font-semibold text-gray-700">Prep (min)</Text>
              </View>
              <TextInput
                className="bg-white border-2 border-gray-200 focus:border-orange-400 rounded-lg px-3 py-2 text-base"
                placeholder="15"
                value={recipeInfo.prepTimeMinutes.toString()}
                onChangeText={(text) => setRecipeInfo(prev => produce(prev, draft => {
                  draft.prepTimeMinutes = parseInt(text) || 0;
                }))}
                keyboardType="numeric"
              />
            </View>
            <View className="flex-1">
              <View className="flex-row items-center mb-1">
                <Timer size={16} className="text-red-600 mr-1" />
                <Text className="text-sm font-semibold text-gray-700">Cook (min)</Text>
              </View>
              <TextInput
                className="bg-white border-2 border-gray-200 focus:border-orange-400 rounded-lg px-3 py-2 text-base"
                placeholder="30"
                value={recipeInfo.cookTimeMinutes.toString()}
                onChangeText={(text) => setRecipeInfo(prev => produce(prev, draft => {
                  draft.cookTimeMinutes = parseInt(text) || 0;
                }))}
                keyboardType="numeric"
              />
            </View>
          </View>
        </View>
      </View>
    </ScrollView>);
}

interface IngredientInput {
  name: string,
  amount: string,
  unit: string
}

/**
 * 
 * @returns 
 */
function IngredientsTab({ recipeInfo, setRecipeInfo }: RecipeSubMenuProps) {
  const [newIngredient, setNewIngredient] = useState<IngredientInput>({
    name: "",
    amount: "",
    unit: "cups"
  });

  const handleAddIngredientAndReset = (input: IngredientInput) => {
    setRecipeInfo(prev => produce(prev, draft => {
      draft.ingredients.push({
        id: Date.now().toString(),
        name: input.name,
        amount: parseFloat(parseFloat(input.amount).toFixed(1)) || 0,
        unit: input.unit,
      });
    }))
    setNewIngredient({ name: '', amount: '', unit: '' });
  }

  const handleRemoveIngredient = (id: string) => {
    setRecipeInfo(prev => produce(prev, draft => {
      draft.ingredients = draft.ingredients.filter(i => i.id !== id)
    }))
  }

  const [unitsOpen, setUnitsOpen] = useState(false);
  const [units, setUnits] = useState<ItemType<ValueType>[]>([]);

  useEffect(() => {
    setUnits([
      /* Dry Volume */
      { label: 'cup', value: 'cup' },
      { label: 'tbsp', value: 'tbsp' },
      { label: 'tsp', value: 'tsp' },

      /* Weight */
      { label: 'g', value: 'g' },
      { label: 'oz', value: 'oz' },
      { label: 'pounds', value: 'lb' },

      /* Wet Volume */
      { label: 'ml', value: 'ml' },

      /* Ingredient Specific */
      { label: 'cloves', value: 'cloves' }
    ])
  }, []);

  return (
    <ScrollView className="flex-1 p-4 pb-20">
      {/* Add Ingredient Card */}
      <View style={{ zIndex: 1000 }} className="bg-gradient-to-br from-lime-50 to-green-50 border-2 border-lime-200 rounded-xl mb-6 shadow-sm">
        <View className="p-4 border-b border-lime-100">
          <View className="flex-row items-center">
            <Plus size={20} className="text-lime-600 mr-2" />
            <Text className="text-lg font-bold text-gray-800">Add Ingredient</Text>
          </View>
        </View>
        <View className="p-4 space-y-4">
          <View>
            <Text className="text-sm font-semibold text-gray-700 mb-1">Ingredient</Text>
            <TextInput
              className="bg-white border-2 border-gray-200 focus:border-lime-400 rounded-lg px-3 py-2 text-base"
              placeholder="e.g., Hoisin sauce"
              value={newIngredient.name}
              onChangeText={(text) => setNewIngredient({ ...newIngredient, name: text })}
            />
          </View>

          <View className="flex-row space-x-4"
            style={{
              zIndex: 1000, // A high z-index to make it appear on top
            }}>
            <View className="flex-1">
              <Text className="text-sm font-semibold text-gray-700 mb-1">Amount</Text>
              <TextInput
                className="bg-white border-2 border-gray-200 focus:border-lime-400 rounded-lg px-3 py-2 text-base"
                placeholder="1"
                value={newIngredient.amount}
                onChangeText={(text) => setNewIngredient({ ...newIngredient, amount: text })}
                keyboardType="numeric"
              />
            </View>


            <View className="flex-1">
              <Text className="text-sm font-semibold text-gray-700 mb-1">Unit</Text>
              <DropDownPicker
                items={units}
                value={newIngredient.unit}
                open={unitsOpen}
                setItems={setUnits}
                setValue={(value) => setNewIngredient(prevIngredient => ({
                  ...prevIngredient,
                  unit: value(prevIngredient)
                }))}
                setOpen={setUnitsOpen}
                dropDownContainerStyle={{
                  backgroundColor: "white",
                  borderColor: '#ccc',
                  borderWidth: 1,
                  borderTopWidth: 0, // Avoid double border between the button and the list
                  zIndex: 1000,
                }} />
            </View>
          </View>

          <TouchableOpacity
            className="bg-gray-500 hover:bg-gray-600 rounded-lg py-3 flex-row items-center justify-center shadow-lg"
            onPress={() => handleAddIngredientAndReset(newIngredient)}
          >
            <Plus size={16} className="text-white mr-2" />
            <Text className="text-white font-semibold text-base">Add Ingredient</Text>
          </TouchableOpacity>
        </View>
      </View>

      {/* Ingredients List */}
      {
        recipeInfo.ingredients.length > 0 && (
          <View className="bg-white border-2 border-green-200 rounded-xl shadow-sm">
            <View className="p-4">
              {recipeInfo.ingredients.map((ingredient) => (
                <View key={ingredient.id} className="flex-row items-center justify-between p-3 bg-white/50 rounded-lg border border-green-100 mb-3">
                  <View className="flex-1">
                    <Text className="font-semibold text-gray-800">{ingredient.name}</Text>
                    <Text className="text-sm text-gray-500">
                      {ingredient.amount} {ingredient.unit}
                    </Text>
                  </View>
                  <TouchableOpacity
                    className="p-2 hover:bg-red-50 rounded-lg"
                    onPress={() => handleRemoveIngredient(ingredient.id)}
                  >
                    <Trash2 size={16} className="text-red-500" />
                  </TouchableOpacity>
                </View>
              ))}
            </View>
          </View>
        )
      }
    </ScrollView >);
}

/** A component for rendering the Instructions portion of the recipe creation flow.
 * This accepts markdown text with a special flavor of parsing for recipes.
 * 
 * @returns A scrollable react component to be placed at the tab layer of the recipe creation screen.
 */
function InstructionsTab({ recipeInfo, setRecipeInfo }: RecipeSubMenuProps) {
  return (<ScrollView className="flex-1 p-4 pb-20">
    {/* Instructions Card */}
    <View className="bg-gradient-to-br from-orange-50 to-yellow-50 border-2 border-orange-200 rounded-xl mb-6 shadow-sm">
      <View className="p-4 border-b border-orange-100">
        <View className="flex-row items-center">
          <StickyNote size={20} className="text-orange-600 mr-2" />
          <Text className="text-lg font-bold text-gray-800">Cooking Instructions</Text>
        </View>
      </View>
      <View className="p-4">
        <TextInput
          className="bg-white border-2 border-gray-200 focus:border-orange-400 rounded-lg px-3 py-2 text-base min-h-48"
          placeholder="Enter step-by-step cooking instructions..."
          value={recipeInfo.instructions}
          onChangeText={text => setRecipeInfo(prev => produce(prev, draft => {
            draft.instructions = text;
          }))}
          multiline
          textAlignVertical="top"
        />
      </View>
    </View>
  </ScrollView>)
}