<template>
  <v-form>
    <v-container>
      <v-row>
        <v-col cols="12" md="12">
          <ValidationProvider v-slot="{ errors }" rules="required" :name="$t('general.name')">
            <v-text-field
              :label="$t('general.name')"
              required
              :value="station.name"
              :error-messages="errors"
              @input="$emit('update:station', { ...station, name: $event })"
            />
          </ValidationProvider>
        </v-col>

        <v-col cols="12" md="12">
          <v-text-field
            :label="$t('stations.position')"
            :value="positionValue"
            @input="$emit('update:station', { ...station, ...getLatLong($event) })"
            @keydown="justNumber"
          >
            <!-- <template v-slot:prepend>
              <v-tooltip top>
                <template v-slot:activator="{ on }">
                  <v-icon small v-on="on">mdi-information-outline</v-icon>
                </template>
                <span>49.836409, 18.233729 nebo 49.8369683N, 18.2297383E </span>
              </v-tooltip>
            </template> -->
          </v-text-field>
        </v-col>
      </v-row>
    </v-container>
  </v-form>
</template>

<script>
import { ValidationProvider } from "vee-validate";
import { getLatLong, getPositionValue, justNumber } from "../../code/helpers/positionHelpers";

export default {
  name: "EditStationModal",
  components: {
    ValidationProvider,
  },
  props: {
    station: {
      type: Object,
      required: true,
    },
  },
  computed: {
    positionValue() {
      return getPositionValue(this.station);
    },
  },

  methods: {
    getLatLong,
    justNumber,
  },
};
</script>
